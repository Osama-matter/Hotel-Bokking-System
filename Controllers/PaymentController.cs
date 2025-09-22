using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class PaymentController : ControllerBase
    {

        iPayment paymentRepo;
        IBooking bookingRepo;
        iBill BillRepo;
        public PaymentController(iPayment paymentRepo , IBooking bookingRepo, iBill billRepo)
        {
            this.paymentRepo = paymentRepo;
            this.bookingRepo = bookingRepo;
            BillRepo=billRepo;
        }

        [HttpGet("Payment Record")]
        public async Task<IActionResult> ShowAll()
        {
            var Record   =await paymentRepo.ShowAll();
            return Ok(Record);
        }


        [HttpGet("Using{ID:int}")]
        public async Task<IActionResult>Getbyid(int  ID  )
        {
            var  Payment  = await paymentRepo.Getbyid(ID);
            var booking = await bookingRepo.GetById(Payment.BookingID);
            if (booking == null)
                return NotFound("Booking not found");
            return Ok(booking);
            
        }

        [HttpPost("Create")]
        [AllowAnonymous]
        public async Task<IActionResult>Create([FromForm]DTO_Payment payment)
        {
            if(ModelState.IsValid)
            {
                var  id  = await paymentRepo.CreatePayment(payment);
                if (id == 0) return BadRequest("Not Found Booking ID");
                // تخليها Paid

                // 🟢 هات بيانات الحجز من الـ Repo
                var booking = await bookingRepo.GetById(payment.BookingID);
                if (booking == null)
                    return NotFound("Booking not found");



                var bill = await BillRepo.GenerateBill(booking, payment);


                // 🟢 توليد PDF
                var pdfBytes = PdfGenerator.GenerateBill(bill);
                return File(pdfBytes, "application/pdf", $"Bill_{id}.pdf");
            }

            return BadRequest(ModelState);

        }


    }
}
