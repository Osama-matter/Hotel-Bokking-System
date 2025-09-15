using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Bokking_System.Models
{
    public class Cls_Reviews
    {
        [Key]
        [Display(Name = "Review ID")]
        public int ID { get; set; }

        [Required]
        public int CustomarID { get; set; }

        [ForeignKey(nameof(CustomarID))]
        public Cls_Customr Customar { get; set; }   // علاقة 1:Many مع العملاء

        [Required]
        public int RoomID { get; set; }

        [ForeignKey(nameof(RoomID))]
        public Cls_Room Room { get; set; }         // علاقة 1:Many مع الغرف

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        [Display(Name = "Rating")]
        public int Rateing { get; set; }

        [Required]
        [MaxLength(1000)]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
    }
}
