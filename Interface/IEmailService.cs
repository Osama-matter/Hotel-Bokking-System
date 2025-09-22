using Hotel_Bokking_System.DTO;

namespace Hotel_Bokking_System.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, string replyTo = null);
        Task SendBookingConfirmationEmailAsync(string customerEmail, Bill_DTO billData);
        Task SendPaymentConfirmationEmailAsync(string customerEmail, Bill_DTO billData);
    }
}
