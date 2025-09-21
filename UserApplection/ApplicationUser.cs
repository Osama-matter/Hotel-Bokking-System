using Hotel_Bokking_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Hotel_Bokking_System.UserApplection
{
    public class ApplicationUser: IdentityUser
    {


       
        public string  Address { get; set; }
         
        public ICollection<Cls_Customr>? _Customrs {  get; set; }

        public ICollection<Cls_Reviews> Reviews { get; set; }

    }
}
