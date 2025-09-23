using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Bokking_System.Repositry
{
    public class ReviewsRepository : GenericRepository<Cls_Reviews>, IReview
    {
        Hotel_dbcontext dbcontext;
        public ReviewsRepository(Hotel_dbcontext context) : base(context)
        {
            dbcontext = context;
        }

        public async Task<List<DTO_Reviews>> ShowAll()
        {
            var reviews = await dbcontext.Reviews
                .Include(r => r.User) // يربط AspNetUsers
                .ToListAsync();

            return reviews.Select(r => new DTO_Reviews
            {
                Rateing = r.Rateing,
                Comment = r.Comment,
                Userid = r.Userid,        // مفتاح المستخدم
                RoomID = r.RoomID,
                ID  = r.ID,
                CreatedAt = r.CreatedAt  ,
                UserName = r.User.UserName
                
            }).ToList();
        }

        public async Task<DTO_Reviews> Create(DTO_Reviews _Reviews)
        {
            var review = new Cls_Reviews        // Use  Cls Not  DTO to save in data base
            {
                Comment = _Reviews.Comment,
                Rateing = _Reviews.Rateing,
                RoomID  = _Reviews.RoomID,
                CreatedAt = DateTime.Now,
                Userid = _Reviews.Userid  
            };

            dbcontext.Reviews.Add(review);
            await dbcontext.SaveChangesAsync();

            return new DTO_Reviews
            {
                Comment = review.Comment,
                Rateing = review.Rateing,
                RoomID = review.RoomID,
                CreatedAt = review.CreatedAt,
                Userid = review.Userid,
                
            };
        }

        public async Task <bool> Update(int id  , DTO_Reviews _Reviews)
        {
            var Review =await dbcontext.Reviews.FirstOrDefaultAsync(e=> e.ID == id);

            if(Review == null)
            {
                return false; 
            }

            Review.Rateing = _Reviews.Rateing;
            Review.Comment = _Reviews.Comment;

            Review.CreatedAt =DateTime.Now;

            dbcontext.Update(Review);
             dbcontext.SaveChanges();
            return true ; 
           
        }

        public async Task<bool> Delete(int  id  )
        {
            var  reviews = await dbcontext.Reviews.FirstOrDefaultAsync(e=> e.ID == id);
            if(reviews != null)
            {
                dbcontext.Reviews.Remove(reviews);
                dbcontext.SaveChanges();
                return true ;
            }
            return false ;
        }


    }
}
