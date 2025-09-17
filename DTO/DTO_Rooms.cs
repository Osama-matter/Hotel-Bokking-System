using static Hotel_Bokking_System.Models.Cls_Room;

namespace Hotel_Bokking_System.DTO
{
    public class DTO_Rooms
    {

        public int RoomID { get; set; }
        public string RoomNumber { get; set; }
        public RoomType Type { get; set; }
        public decimal PricePerNight { get; set; }
        public string Description { get; set; }
        public RoomStatus Status { get; set; }

        public List<DTO_Reviews> Reviews { get; set; }
        public List<DTO_RoomImages> Images { get; set; }
    }
}
