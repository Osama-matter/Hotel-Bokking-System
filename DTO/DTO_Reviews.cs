namespace Hotel_Bokking_System.DTO
{
    public class DTO_Reviews
    {
        public int ID { get; set; }
        public string ? Userid { get; set; }
        public int RoomID { get; set; }

        public string?  UserName  { get; set; }
        public int Rateing { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
