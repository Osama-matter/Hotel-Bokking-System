using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Repositry;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {

        IBooking _bookingRepo;
        public BookingController(IBooking bookingrepo)
        {
            this._bookingRepo = bookingrepo;
        }

        //  Get All Bookings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingRepo.GetAll();
            return Ok(bookings);
        }

        //  Get Booking by ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingRepo.GetById(id);
            if (booking == null) return NotFound(" Booking not found.");
            return Ok(booking);
        }

        //  Create New Booking
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DTO_Booking request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _bookingRepo.Create(request);// create  int  method to retrun id  to use  it  in hotel resaption 
            return Ok(new { Message = " Booking created successfully", BookingId = id });   // return bboking id 
        }

        //  Update Booking
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] DTO_Booking request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _bookingRepo.Update(id, request);
            if (!updated) return NotFound("Booking not found.");

            return Ok(" Booking updated successfully");
        }


        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> GetBookingDetails(int id)
        {
            var bookingDetails = await _bookingRepo.GetBookingDetailsByIdAsync(id);

            if (bookingDetails == null)
                return NotFound(new { Message = $"Booking with ID {id} not found." });

            return Ok(bookingDetails);
        }


        //  Delete Booking
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _bookingRepo.Delete(id);
            if (!deleted) return NotFound("Booking not found.");

            return Ok("Booking deleted successfully");
        }


    }
}
