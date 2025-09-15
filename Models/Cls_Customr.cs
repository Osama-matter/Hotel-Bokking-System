using Hotel_Bokking_System.UserApplection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Bokking_System.Models
{
    public class Cls_Customr
    {

        [Key]
        [Display(Name ="Customr ID")]
        public int CustomarID { get; set; }
        [Required]
       
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }  // Navigation property
        [Required]

        [Display(Name ="Customr Name ")]
        public string   FullName { get; set; }

        [Required]
        [Display(Name ="Address")]
        public string  Address { get; set; }
        [Required]

        
        public int LoyaltyPoints { get; set; }  =  0;   
       
    }
}
