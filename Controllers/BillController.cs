using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {

        iBill BillRepo;
        iPayment PaymentRepo;
        IBooking BookingRepo; 


        public  BillController(iBill iBill , IBooking booking , iPayment payment)
        {
            BillRepo = iBill; 
            BookingRepo = booking;
            PaymentRepo = payment;
        }


        [HttpPost("Generate Bill")]

        public async Task<IActionResult> Generatebill(int  Bookingid , int  paymentID)
        {
            
            var Payment = await PaymentRepo.Getbyid(paymentID); 
            var Booking = await BookingRepo.GetById(Bookingid);
            // Check if either not found OR not related
            if (Payment == null || Booking == null || Booking.BookingID != Payment.BookingID)
            {
                return BadRequest($" No Bill found: PaymentId={paymentID} is not related to BookingId={Bookingid}");
            }
            var  bill =  await BillRepo.GenerateBill(Booking, Payment);

            // 🟢 توليد PDF
            var pdfBytes = PdfGenerator.GenerateBill(bill);
            return File(pdfBytes, "application/pdf", $"Bill_.pdf");
        }
    }
}
