using Hotel_Bokking_System.Models;

namespace Hotel_Bokking_System.DTO
{
    public class DTO_Payment
    {

        public int PaymentID { get; set; }
        public int BookingID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public Cls_Payments.PaymentMethod Method { get; set; }   // نحول Enum لـ string
        public Cls_Payments.PaymentStatus Status { get; set; }   // نحول Enum لـ string
    }
}
