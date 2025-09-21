
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;

namespace Hotel_Bokking_System.Interface
{
    public interface IReview : IGenericRepository<Cls_Reviews>

    {

        Task<List<DTO_Reviews>> ShowAll();
        Task<DTO_Reviews> Create(DTO_Reviews _Reviews);

        Task<bool> Update(int id, DTO_Reviews _Reviews);

        Task<bool> Delete(int id);
    }
}
