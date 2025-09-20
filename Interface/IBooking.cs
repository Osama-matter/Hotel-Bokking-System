using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;

namespace Hotel_Bokking_System.Interface
{
    public interface IBooking  : IGenericRepository<Cls_Booking> 
    {

  

        Task<List<DTO_Booking>> GetAll();
        Task<DTO_Booking?> GetById(int id);
        Task<int> Create(DTO_Booking dto);
        Task<bool> Update(int id, DTO_Booking dto);
        Task<bool> Delete(int id);
        Task<DTO_BookingDetails> GetBookingDetailsByIdAsync(int ID); 
    }
}
