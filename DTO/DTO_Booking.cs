using Hotel_Bokking_System.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Hotel_Bokking_System.Models.Cls_Booking;

namespace Hotel_Bokking_System.DTO
{
    public class DTO_Booking
    {

        public int BookingID { get; set; }

   

        public int CustomarID { get; set; }

     


        public string CustomrName { get; set; }
        public int RoomID { get; set; }

        public string RoomNumber  { get; set; }

        public DateTime CheckIn { get; set; }


        public DateTime CheckOut { get; set; }

        public DateTime Created { get; set; } 


        public BookingStatus Status { get; set; } = BookingStatus.Pending;



    }
}
