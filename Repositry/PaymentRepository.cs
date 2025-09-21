using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Bokking_System.Repositry
{
    public class PaymentRepository : GenericRepository<Cls_Payments>, iPayment
    {
        Hotel_dbcontext dbcontext;
        public PaymentRepository(Hotel_dbcontext context) : base(context)
        {
            dbcontext = context;
        }


        public async Task<List<DTO_Payment>> ShowAll()
        {

           var PaymentRecord =await dbcontext.Payments
                .Include(e=>e.Booking)
                .ToListAsync();

           return PaymentRecord.Select(e=> new DTO_Payment
           {
               PaymentID = e.PaymentID,
               PaymentDate = e.PaymentDate,
               Status = e.Status,
               Method = e.Method,
               Amount = e.Amount,
               BookingID = e.BookingID,
               
           }).ToList(); 
        }


        public async Task<DTO_Payment> Getbyid(int Id )
        {


            var PaymentRecord = await dbcontext.Payments
                 .Include(e => e.Booking)
                 .FirstOrDefaultAsync(e=> e.PaymentID == Id);

            if (PaymentRecord == null) return new DTO_Payment(); 
            return new DTO_Payment { 
                PaymentID   = PaymentRecord.PaymentID,
                PaymentDate = PaymentRecord.PaymentDate,
                BookingID= PaymentRecord.BookingID,
                Status = PaymentRecord.Status,
                Method = PaymentRecord.Method,
                Amount = PaymentRecord.Amount,
            };


        }
        public bool IsValidBookingId(int id)
        {
            return dbcontext.Bookings.Any(e => e.BookingID == id);
        }

        public async Task<int > CreatePayment(DTO_Payment _Payment)
        {
            if (IsValidBookingId(_Payment.BookingID)  == true  )
            {
                var Payment = new Cls_Payments
                {
                    Amount=_Payment.Amount,
                    BookingID=_Payment.BookingID,
                    Method =_Payment.Method,
                    PaymentDate=_Payment.PaymentDate,
                    Status = _Payment.Status,
                    
                };
                dbcontext.Payments.Add(Payment);
                dbcontext.SaveChanges();
                return Payment.PaymentID;
            }
            else return 0;
           
        }



    }
}
