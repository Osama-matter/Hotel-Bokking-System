using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;

namespace Hotel_Bokking_System.Repositry
{
    public class BillRepostitory  :iBill
    {


        Hotel_dbcontext dbcontext;
        public BillRepostitory(Hotel_dbcontext context)
        {
            dbcontext = context;
        }


        public async Task<Bill_DTO> GenerateBill(DTO_Booking booking, DTO_Payment Payment)
        {
            var bill = new Bill_DTO
            {
                BookingID = booking.BookingID,
                CustomrName = booking.CustomrName,   // ← من DTO
                RoomNumber = booking.RoomNumber,     // ← من DTO
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Amount = Payment.Amount,
                paymentStatus = Payment.Status,
                Created = DateTime.Now
            };

            return bill;
        }

    }
}
