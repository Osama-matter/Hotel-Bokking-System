using Hotel_Bokking_System.Models;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Bokking_System.DTO
{
    public class DTO_CreateRoom
    {



            public int RoomID { get; set; }

  
            public string RoomNumber { get; set; }

       
            public Cls_Room.RoomType  Type { get; set; }

            public decimal PricePerNight { get; set; }

            public string Description { get; set; }


            public Cls_Room.RoomStatus  Status { get; set; }



        public List<IFormFile> Images { get; set; } = new List<IFormFile>(); // 
    }

}



