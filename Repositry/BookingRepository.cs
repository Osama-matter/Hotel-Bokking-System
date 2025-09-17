using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Bokking_System.Repositry
{
    public class BookingRepository : GenericRepository<Cls_Booking>, IBooking
    {

        Hotel_dbcontext dbcontext;
        public BookingRepository(Hotel_dbcontext context) : base(context)
        {
            dbcontext = context;
        }

        public async Task<IEnumerable<Cls_Booking>> GetAllWithDetailsAsync()
        {
            return  await dbcontext.Bookings.Include(r=> r.Room).Include(c=> c.Customer.CustomarID).ToListAsync();
        }

        public Task<IEnumerable<Cls_Booking>> GetUsingIDWithDetailsAsync(int ID)
        {
            throw new NotImplementedException();
        }
    }
}
