using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;


namespace Hotel_Bokking_System.Repositry
{
    public class RoomsRepository : GenericRepository<Cls_Room>, IRoom
    {
        private readonly Hotel_dbcontext dbcontext;

        public RoomsRepository(Hotel_dbcontext context) : base(context)
        {
            dbcontext = context;
        }

        // إظهار كل الغرف
        public async Task<List<DTO_Rooms>> ShowRooms()
        {
            var rooms = await dbcontext.Rooms
                .Include(e => e.iMages)
                .Include(e => e.Reviews)
                .ToListAsync();

            return rooms.Select(r => new DTO_Rooms
            {
                RoomNumber = r.RoomNumber,
                Type = r.Type,
                PricePerNight = r.PricePerNight,
                Description = r.Description,
                Status = r.Status,

                Images = r.iMages?.Select(img => new DTO_RoomImages
                {
                    Id = img.Id,
                    ImagePath = img.ImagePath
                }).ToList(),

                Reviews = r.Reviews?.Select(rv => new DTO_Reviews
                {
                    ID = rv.ID,
                    CustomarID = rv.CustomarID,
                    RoomID = rv.RoomID,
                    Rateing = rv.Rateing,
                    Comment = rv.Comment,
                    CreatedAt = rv.CreatedAt
                }).ToList()
            }).ToList();
        }

        // إظهار غرفة معينة باستخدام ID
        public async Task<DTO_Rooms?> ShowRoomById(int id)
        {
            var room = await dbcontext.Rooms
                .Include(e => e.iMages)
                .Include(e => e.Reviews)
                .FirstOrDefaultAsync(e => e.RoomID == id);

            if (room == null) return null;

            return new DTO_Rooms
            {
                RoomNumber = room.RoomNumber,
                Type = room.Type,
                PricePerNight = room.PricePerNight,
                Description = room.Description,
                Status = room.Status,

                Images = room.iMages?.Select(img => new DTO_RoomImages
                {
                    Id = img.Id,
                    ImagePath = img.ImagePath
                }).ToList(),

                Reviews = room.Reviews?.Select(rv => new DTO_Reviews
                {
                    ID = rv.ID,
                    CustomarID = rv.CustomarID,
                    RoomID = rv.RoomID,
                    Rateing = rv.Rateing,
                    Comment = rv.Comment,
                    CreatedAt = rv.CreatedAt
                }).ToList()
            };
        }




        public async Task<DTO_Rooms> CreateRoom(DTO_CreateRoom dto)
        {


            if (await dbcontext.Rooms.AnyAsync(r => r.RoomNumber == dto.RoomNumber))
            {
                throw new Exception("Room number already exists.");
            }

            var room = new Cls_Room                             /// take data  from DTO put  in class  ////
            {
                RoomNumber = dto.RoomNumber,
                Type = dto.Type,
                PricePerNight = dto.PricePerNight,
                Description = dto.Description,
                Status = dto.Status,
                Capacity = dto.Capacity,
                bedType=dto.bedType,
                Floor = dto.Floor,
                iMages = new List<Cls_RoomIMages>()
            };

            /// To Save  Images as  Servier Side  

            //  إنشاء مسار حفظ الصور داخل wwwroot/images/rooms.

            //التأكد من وجود المجلد أو إنشاؤه إذا لم يكن موجودًا.

            //التحقق من وجود صور في الـ DTO.

            //لكل صورة: توليد اسم فريد → حفظها على القرص → إضافة مسارها لكائن الغرفة.





            // مسار المجلد النسبي
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/rooms");

            // التأكد من وجود المجلد، وإن لم يكن موجود يتم إنشاؤه
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // حفظ الصور
            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var file in dto.Images)
                {
                    // توليد اسم فريد للملف مع امتداد الصورة
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(folderPath, fileName);

                    // حفظ الصورة على القرص
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // إضافة الصورة للقائمة المرتبطة بالغرفة
                    room.iMages.Add(new Cls_RoomIMages
                    {
                        ImagePath = "/images/rooms/" + fileName // مسار نسبي للعرض في الواجهة
                    });
                }
            }


            dbcontext.Rooms.Add(room);
            await dbcontext.SaveChangesAsync();

            return new DTO_Rooms
            {
                RoomID=room.RoomID,
                RoomNumber = room.RoomNumber,
                Type = room.Type,
                PricePerNight = room.PricePerNight,
                Description = room.Description,
                Status = room.Status,
                Images = room.iMages.Select(img => new DTO_RoomImages
                {
                    Id = img.Id,
                    ImagePath = img.ImagePath
                }).ToList(),
                Reviews = new List<DTO_Reviews>()
            };
        }

       
        public async Task<DTO_Rooms> Edit(int ID, DTO_CreateRoom DTO_Request)
        {
            Cls_Room _Room = await dbcontext.Rooms.FirstOrDefaultAsync(e => e.RoomID == ID);

            if (_Room.RoomNumber == DTO_Request.RoomNumber)
            {
                throw new Exception("Room number already exists.");
            }


            if (_Room != null)
            {
                _Room.RoomNumber = DTO_Request.RoomNumber;
                _Room.Type = DTO_Request.Type;
                _Room.Status = DTO_Request.Status;
                _Room.Description = DTO_Request.Description;
                _Room.PricePerNight = DTO_Request.PricePerNight;

                // مسح الصور القديمة واستبدالها بالجديدة
                _Room.iMages = new List<Cls_RoomIMages>();

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/rooms");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                if (DTO_Request.Images != null && DTO_Request.Images.Any())
                {
                    foreach (var file in DTO_Request.Images)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(folderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        _Room.iMages.Add(new Cls_RoomIMages
                        {
                            ImagePath = "/images/rooms/" + fileName
                        });
                    }
                }

                dbcontext.Rooms.Update(_Room);
                await dbcontext.SaveChangesAsync();

                return new DTO_Rooms
                {
                    RoomID = _Room.RoomID,
                    RoomNumber = _Room.RoomNumber,
                    Type = _Room.Type,
                    PricePerNight = _Room.PricePerNight,
                    Description = _Room.Description,
                    Status = _Room.Status,
                    Images = _Room.iMages.Select(img => new DTO_RoomImages
                    {
                        Id = img.Id,
                        ImagePath = img.ImagePath
                    }).ToList(),
                    Reviews = new List<DTO_Reviews>()
                };
            }

            return null; // إذا لم توجد الغرفة
        }


        // show  romm Detiles 
       
        public async Task<DTO_Rooms?> ShowRoomDetiles(int id)
        {
            var room = await dbcontext.Rooms
                .Include(e => e.iMages)
                .Include(e => e.Reviews)
                .FirstOrDefaultAsync(e => e.RoomID == id);

            if (room == null) return null;

            return new DTO_Rooms
            {
                RoomNumber = room.RoomNumber,
                Type = room.Type,
                PricePerNight = room.PricePerNight,
                Description = room.Description,
                Status = room.Status,
                bedType=room.bedType,
                Capacity= room.Capacity,
                Floor=room.Floor,

                Images = room.iMages?.Select(img => new DTO_RoomImages
                {
                    Id = img.Id,
                    ImagePath = img.ImagePath
                }).ToList(),

                Reviews = room.Reviews?.Select(rv => new DTO_Reviews
                {
                    ID = rv.ID,
                    CustomarID = rv.CustomarID,
                    RoomID = rv.RoomID,
                    Rateing = rv.Rateing,
                    Comment = rv.Comment,
                    CreatedAt = rv.CreatedAt
                }).ToList()
            };
        }






        public async Task<List<DTO_Rooms>> FindUsingData(Cls_Room.RoomStatus? status, Cls_Room.BedType? bedType, int? Floor, Cls_Room.RoomType? roomType, decimal? priceperNight)
        {
            var query = dbcontext.Rooms       // Qery Requerment 
           .AsQueryable();


            if (Floor.HasValue)                            // Collect  Query  Data  (Component)
                query = query.Where(r => r.Floor == Floor.Value);          

            if(bedType.HasValue)
                query = query.Where(r=> r.bedType == bedType.Value);
            if (roomType.HasValue)
                query = query.Where(r => r.Type == roomType.Value);

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            if (priceperNight.HasValue)
                query = query.Where(r => r.PricePerNight <= priceperNight.Value);

            var rooms = await query                                     // Select  Data to  Room have  same  requerment 
            .Select(r => new DTO_Rooms
            {
                RoomNumber = r.RoomNumber,
                Type = r.Type,
                PricePerNight = r.PricePerNight,
                Description = r.Description,
                Status = r.Status,
                Images = r.iMages.Select(img => new DTO_RoomImages
                {
                    Id = img.Id,
                    ImagePath = img.ImagePath
                }).ToList(),
                Reviews = r.Reviews.Select(rv => new DTO_Reviews
                {
                    ID = rv.ID,
                    CustomarID = rv.CustomarID,
                    RoomID = rv.RoomID,
                    Rateing = rv.Rateing,
                    Comment = rv.Comment,
                    CreatedAt = rv.CreatedAt
                }).ToList()
            })
            .ToListAsync();


            if (rooms ==null || rooms.Count==0)   // Check Nullable of  Query 
            {
                return new List<DTO_Rooms>();
            }


            return rooms;   // return Data  if  found  

        }


            
    }

}
