using Hotel_Bokking_System.UserApplection;

namespace Hotel_Bokking_System.DTO
{
    public class DTO_Customer
    {

        public int CustomarID { get; set; }

        public string?  UserId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }


        public string Email { get; set; }
        public int LoyaltyPoints { get; set; }
    }
}
