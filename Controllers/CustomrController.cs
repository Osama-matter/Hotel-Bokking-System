using Hotel_Bokking_System.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomrController : ControllerBase
    {

        iCustomr CustomerRepo; 
        public  CustomrController (iCustomr _customerRepo)
        {
            CustomerRepo = _customerRepo;
        }

    }
}
