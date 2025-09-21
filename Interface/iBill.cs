using Hotel_Bokking_System.DTO;

namespace Hotel_Bokking_System.Interface
{
    public interface iBill
    {

        public  Task<Bill_DTO> GenerateBill(DTO_Booking booking, DTO_Payment Payment); 
    }
}
