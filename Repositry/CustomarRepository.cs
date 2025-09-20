using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Bokking_System.Repositry
{
    public class CustomarRepository : GenericRepository<Cls_Customr>, iCustomr
    {
        Hotel_dbcontext dbcontext;
        public CustomarRepository(Hotel_dbcontext context) : base(context)
        {
            dbcontext = context;
        }

        public async Task<IEnumerable<Cls_Customr>> GetAll()
        {
            return await dbcontext.Set<Cls_Customr>().ToListAsync();
        }

        public async Task<DTO_Customer> Create(DTO_Customer dto_customer)
        {
            if(dto_customer == null)
            {
                return dto_customer = new DTO_Customer();
            }

            var _Customr = new Cls_Customr
            { 
                UserId=dto_customer.UserId,
                FullName=dto_customer.FullName,
                PhoneNumber=dto_customer.PhoneNumber,
                Address=dto_customer.Address,
                LoyaltyPoints=dto_customer.LoyaltyPoints +5 ,
                Email=dto_customer.Email

            };

            dbcontext.Add(_Customr);
            dbcontext.SaveChanges();


            return new DTO_Customer
            { 
                Email = _Customr.Email,
                PhoneNumber = _Customr.PhoneNumber,
                Address = _Customr.Address,
                FullName = _Customr.FullName,
                UserId  = dto_customer.UserId,
                LoyaltyPoints= dto_customer.LoyaltyPoints,

            };
        }


        public async Task<DTO_Customer> Update(int  ID , DTO_Customer dto_customer)
        {
            if (dto_customer == null)
            {
                return dto_customer = new DTO_Customer();
            }

            var _Customr = await dbcontext.Customers
                .FirstOrDefaultAsync(c => c.CustomarID ==ID );   // take data from database  to Edit it  

            if (_Customr == null)
            {
                throw new Exception("Customer not found");     // Thro exaption if  not  found  
            }

            // 2️⃣ عدّل القيم
            _Customr.FullName = dto_customer.FullName;     // update data 
            _Customr.PhoneNumber = dto_customer.PhoneNumber;
            _Customr.Address = dto_customer.Address;
            _Customr.LoyaltyPoints = dto_customer.LoyaltyPoints;
            _Customr.Email = dto_customer.Email;
            _Customr.UserId = dto_customer.UserId;

            // 3️⃣ احفظ التغييرات
            await dbcontext.SaveChangesAsync();    // save changres

            // 4️⃣ رجع DTO
            return new DTO_Customer    // return data  after  Edit  
            {
                Email = _Customr.Email,
                PhoneNumber = _Customr.PhoneNumber,
                Address = _Customr.Address,
                FullName = _Customr.FullName,
                UserId = _Customr.UserId,
                LoyaltyPoints = (int)_Customr.LoyaltyPoints,

            };
        }

        public async Task<bool> Delete(int  ID)
        {
            var Customr = dbcontext.Customers.FirstOrDefault(e=> e.CustomarID == ID);
            if(Customr == null)
            {
                return false;
            }
            dbcontext.Customers.Remove(Customr);
            return true;
        }

    }
}
