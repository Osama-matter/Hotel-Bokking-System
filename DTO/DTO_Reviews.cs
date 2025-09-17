namespace Hotel_Bokking_System.DTO
{
    public class DTO_Reviews
    {
        public int ID { get; set; }
        public int CustomarID { get; set; }
        public int RoomID { get; set; }
        public int Rateing { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
