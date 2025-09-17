using Hotel_Bokking_System.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {

        IBooking bookingrepo;
        public BookingController(IBooking bookingrepo)
        {
            this.bookingrepo = bookingrepo;
        }


    }
}
