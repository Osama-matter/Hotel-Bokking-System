
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;

namespace Hotel_Bokking_System.Interface
{
    public interface iPayment : IGenericRepository<Cls_Payments>
    {
        Task<int > CreatePayment(DTO_Payment _Payment);

        Task<List<DTO_Payment>> ShowAll();

        Task<DTO_Payment> Getbyid(int Id);


    }
}
