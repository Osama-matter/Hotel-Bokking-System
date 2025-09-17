using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;

namespace Hotel_Bokking_System.Repositry
{
    public class PaymentRepository : GenericRepository<Cls_Payments>, iPayment
    {
        Hotel_dbcontext dbcontext;
        public PaymentRepository(Hotel_dbcontext context) : base(context)
        {
            dbcontext = context;
        }

    }
}
