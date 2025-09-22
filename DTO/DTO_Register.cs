using System.ComponentModel;

namespace Hotel_Bokking_System.DTO
{
    public class DTO_Register
    {


        public string UserName { get; set; }
        [PasswordPropertyText]
        
        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
