using Hotel_Bokking_System.Models;

namespace Hotel_Bokking_System.DTO
{
    public class Bill_DTO
    {

        public int BookingID { get; set; }


        public string CustomrName { get; set; }


        public string RoomNumber { get; set; }

        public DateTime CheckIn { get; set; }


        public DateTime CheckOut { get; set; }

        public decimal Amount { get; set; }

        public Cls_Payments.PaymentStatus    paymentStatus { get; set; }

        public DateTime Created { get; set; }

    }
}
