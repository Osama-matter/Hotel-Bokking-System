namespace Hotel_Bokking_System.DTO
{
    public class DTO_Payment
    {

        public int PaymentID { get; set; }
        public int BookingID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Method { get; set; }   // نحول Enum لـ string
        public string Status { get; set; }   // نحول Enum لـ string
    }
}
