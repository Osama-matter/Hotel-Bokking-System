namespace Hotel_Bokking_System.DTO
{
    public class DTO_BookingDetails
    {

        public int BookingID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string BookingStatus { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime Created { get; set; }
    }
}
