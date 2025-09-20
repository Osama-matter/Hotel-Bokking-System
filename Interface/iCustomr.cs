
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;

namespace Hotel_Bokking_System.Interface
{
    public interface iCustomr : IGenericRepository<Cls_Customr>
    {
        Task<IEnumerable<Cls_Customr>> GetAll();
        Task<DTO_Customer> Create(DTO_Customer dto_customer);
        Task<DTO_Customer> Update(int ID, DTO_Customer dto_customer);

        Task<bool> Delete(int ID);
    }
}
