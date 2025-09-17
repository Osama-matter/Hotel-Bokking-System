using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Bokking_System.Models
{
    public class Cls_RoomIMages
    {
        [Key]
        public int Id { get; set; }
        public string ImagePath { get; set; }


        // ا
        // لمفتاح الأجنبي
        public int RoomId { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Cls_Room Room { get; set; }
    }
}
