using Hotel_Bokking_System.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        iPayment paymentRepo; 
        public PaymentController(iPayment paymentRepo)
        {
            this.paymentRepo = paymentRepo;
        }


    }
}
