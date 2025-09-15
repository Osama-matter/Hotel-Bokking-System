using System.ComponentModel.DataAnnotations;

namespace Hotel_Bokking_System.Models
{
    public class Cls_Room
    {
        public enum RoomStatus
        {
            Available,    // متاحة
            Booked,       // محجوزة
            Maintenance   // تحت الصيانة
        }

        public enum RoomType
        {
            Single,   // سرير واحد
            Double,   // سريرين
            Suite,    // جناح
            Deluxe,   // ديلوكس
            Family    // عائلي
        }

        [Key]
        [Display(Name = "Room ID")]
        public int RoomID { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }

        [Required]
        [Display(Name = "Type")]
        public RoomType Type { get; set; }

        [Required]
        [Display(Name = "Price Per Night")]
        public decimal PricePerNight { get; set; }

        [Required]
        [Display(Name = "Status")]
        public RoomStatus Status { get; set; }

        // العلاقات
        public ICollection<Cls_Booking> Bookings { get; set; }
        public ICollection<Cls_Reviews> Reviews { get; set; }
    }
}
