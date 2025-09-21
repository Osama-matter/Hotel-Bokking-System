using Hotel_Bokking_System.Controllers;
using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Bokking_System.Repositry
{
    public class BookingRepository : GenericRepository<Cls_Booking>, IBooking
    {

        Hotel_dbcontext dbcontext;
        public BookingRepository(Hotel_dbcontext context) : base(context)
        {
            dbcontext = context;
        }

        public async Task<List<DTO_Booking>> GetAll()
        {
            var bookings = await dbcontext.Bookings
                .Include(e => e.Room)
                .Include(e => e.Customer)
                .ToListAsync();

            return bookings.Select(b => new DTO_Booking
            {
                BookingID = b.BookingID,
                CustomarID = b.CustomarID,
                RoomID =  b.RoomID,

                CustomrName = b.Customer.FullName,
                RoomNumber = b.Room.RoomNumber,

                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                Status = b.Status,
                Created = b.Created
            }).ToList();
        }

        public async Task<DTO_Booking?> GetById(int id)
        {
            var booking = await dbcontext.Bookings
                .Include(e => e.Room)
                .Include(e => e.Customer)
                .FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking == null) return null;

            return new DTO_Booking
            {
                BookingID = booking.BookingID,
                CustomarID = booking.CustomarID,
                RoomID = booking.RoomID,
                CustomrName = booking.Customer.FullName,
                RoomNumber = booking.Room.RoomNumber,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Status = booking.Status,
                Created = booking.Created
            };
        }





        public async Task<DTO_BookingDetails> GetBookingDetailsByIdAsync(int ID)
        {
            var booking = await dbcontext.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .Include(b => b.Payments) // لازم تكون ICollection<Cls_Payments>
                .FirstOrDefaultAsync(e => e.BookingID == ID);

            if (booking == null)
                return null;

            return new DTO_BookingDetails
            {
                BookingID = booking.BookingID,
                CustomerName = booking.Customer.FullName,
                CustomerPhone = booking.Customer.PhoneNumber,
                RoomNumber = booking.Room.RoomNumber,
                RoomType = booking.Room.Type.ToString(),
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                BookingStatus = booking.Status.ToString(),
                PaymentAmount = booking.Payments?.FirstOrDefault()?.Amount,  // أول عملية دفع فقط
                PaymentMethod = booking.Payments?.FirstOrDefault()?.Method.ToString(),
                PaymentStatus = booking.Payments?.FirstOrDefault()?.Status.ToString(),
                Created = booking.Created
            };
        }




        public async Task<int> Create(DTO_Booking dto)
        {



            //Validation

            await BookingValidator.ValidateAsync(dto, dbcontext);

            var booking = new Cls_Booking
            {
                CustomarID = dto.CustomarID,
                RoomID = dto.RoomID,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                Status = dto.Status = Cls_Booking.BookingStatus.Pending,
                Created = DateTime.Now,
                
            };

            dbcontext.Bookings.Add(booking);
            await dbcontext.SaveChangesAsync();

            return booking.BookingID;
        }
        public async Task<bool> Update(int id, DTO_Booking dto)
        {

            var booking = await dbcontext.Bookings.FindAsync(id);       // find Booking   
            if (booking == null) return false;



            //Validation

            await BookingValidator.ValidateAsync(dto, dbcontext , id);

            booking.CustomarID = dto.CustomarID;
            booking.RoomID = dto.RoomID;
            booking.CheckIn = dto.CheckIn;
            booking.CheckOut = dto.CheckOut;
            booking.Status = dto.Status;
            booking.Created = dto.Created;

            dbcontext.Bookings.Update(booking);
            await dbcontext.SaveChangesAsync();

            return true;
        }

        // ✅ Delete
        public async Task<bool> Delete(int id)
        {
            var booking = await dbcontext.Bookings.FindAsync(id);
            if (booking == null) return false;

            dbcontext.Bookings.Remove(booking);
            await dbcontext.SaveChangesAsync();

            return true;
        }

    }
}
