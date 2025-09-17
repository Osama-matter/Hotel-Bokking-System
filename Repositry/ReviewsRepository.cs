using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;

namespace Hotel_Bokking_System.Repositry
{
    public class ReviewsRepository : GenericRepository<Cls_Reviews>, IReview
    {
        Hotel_dbcontext dbcontext;
        public ReviewsRepository(Hotel_dbcontext context) : base(context)
        {
            dbcontext = context;
        }

    }
}
