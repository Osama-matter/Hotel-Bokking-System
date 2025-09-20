using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Bokking_System.Models
{
    public class Cls_Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]

        public int CustomarID { get; set; }
        [ForeignKey(nameof(CustomarID))]
        public Cls_Customr Customer { get; set; }

        [Required]


        public int RoomID { get; set; }
        [ForeignKey(nameof(RoomID))]
        public Cls_Room Room { get; set; }


        [Required]

        [Display(Name ="Check In Date")]
        public DateTime CheckIn { get; set; }

        [Required]

        [Display(Name ="Check Out Date")]
        public DateTime CheckOut { get; set; }


        public DateTime Created { get; set; }= DateTime.Now;
        public enum BookingStatus
        {
            Pending,     // لسه جديد
            Confirmed,   // اتأكد
            CheckedIn,   // العميل دخل
            CheckedOut,  // العميل خرج

            Cancelled,   // اتلغى
            NoShow       // محضرش
        }

        [Display(Name = "Booking Status")]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;


        public ICollection<Cls_Payments>? Payments { get; set; }
    }
}
