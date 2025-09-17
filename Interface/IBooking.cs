using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;

namespace Hotel_Bokking_System.Interface
{
    public interface IBooking  : IGenericRepository<Cls_Booking> 
    {

        Task<IEnumerable<Cls_Booking>> GetAllWithDetailsAsync();

        Task<IEnumerable<Cls_Booking>> GetUsingIDWithDetailsAsync( int  ID );

    }
}
