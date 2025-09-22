using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Hotel_Bokking_System.Repositry
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, string replyTo = null)
        {
            var host = _config["Smtp:Host"] ?? "smtp.gmail.com";
            var portRaw = _config["Smtp:Port"];
            var enableSslRaw = _config["Smtp:EnableSsl"];
            var from = _config["Smtp:From"];
            var fromName = _config["Smtp:FromName"] ?? "Hotel Booking System";
            var username = _config["Smtp:Username"] ?? from;
            var password = _config["Smtp:Password"];

            if (string.IsNullOrWhiteSpace(from))
                throw new InvalidOperationException("SMTP 'From' address is not configured (Smtp:From).");
            if (string.IsNullOrWhiteSpace(password))
                _logger.LogWarning("SMTP password is empty. Email sending will likely fail.");

            int port = int.TryParse(portRaw, out var parsedPort) ? parsedPort : 587;
            bool enableSsl = bool.TryParse(enableSslRaw, out var parsedSsl) ? parsedSsl : true;

            using var smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 30000
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(from, fromName, Encoding.UTF8),
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            foreach (var addr in (to ?? string.Empty).Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(addr.Trim());
            }

            if (mailMessage.To.Count == 0)
                throw new ArgumentException("No valid recipient address provided.", nameof(to));

            if (!string.IsNullOrWhiteSpace(replyTo))
            {
                try { mailMessage.ReplyToList.Add(new MailAddress(replyTo)); }
                catch { /* ignore invalid reply-to */ }
            }

            try
            {
                _logger.LogInformation("Sending email to {To}", string.Join(",", mailMessage.To));
                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully.");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "SMTP error while sending email to {To}", string.Join(",", mailMessage.To));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending email to {To}", string.Join(",", mailMessage.To));
                throw;
            }
        }

        public async Task SendBookingConfirmationEmailAsync(string customerEmail, Bill_DTO billData)
        {
            if (string.IsNullOrWhiteSpace(customerEmail))
                throw new ArgumentException("Customer email is required", nameof(customerEmail));

            if (billData == null)
                throw new ArgumentNullException(nameof(billData));

            string subject = $"Booking Confirmation - #{billData.BookingID}";
            string body = GenerateBookingConfirmationEmailBody(billData);

            await SendEmailAsync(customerEmail, subject, body);

            _logger.LogInformation("Booking confirmation email sent for BookingID: {BookingID}", billData.BookingID);
        }

        public async Task SendPaymentConfirmationEmailAsync(string customerEmail, Bill_DTO billData)
        {
            if (string.IsNullOrWhiteSpace(customerEmail))
                throw new ArgumentException("Customer email is required", nameof(customerEmail));

            if (billData == null)
                throw new ArgumentNullException(nameof(billData));

            string subject = $"Payment Confirmation - Booking #{billData.BookingID}";
            string body = GeneratePaymentConfirmationEmailBody(billData);

            await SendEmailAsync(customerEmail, subject, body);

            _logger.LogInformation("Payment confirmation email sent for BookingID: {BookingID}", billData.BookingID);
        }

        private string GenerateBookingConfirmationEmailBody(Bill_DTO billData)
        {
            var paymentStatusText = billData.paymentStatus.ToString();
            var paymentStatusColor = billData.paymentStatus == Hotel_Bokking_System.Models.Cls_Payments.PaymentStatus.Paid
                ? "#28a745" : "#dc3545";
            var totalNights = (billData.CheckOut.Date - billData.CheckIn.Date).Days;

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Booking Confirmation</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #007bff; color: white; text-align: center; padding: 20px; border-radius: 5px 5px 0 0; }}
        .content {{ background-color: #f8f9fa; padding: 20px; border: 1px solid #dee2e6; }}
        .booking-details {{ background-color: white; padding: 15px; margin: 15px 0; border-radius: 5px; border: 1px solid #dee2e6; }}
        .detail-row {{ display: flex; justify-content: space-between; margin: 10px 0; padding: 5px 0; border-bottom: 1px solid #eee; }}
        .detail-label {{ font-weight: bold; color: #555; }}
        .detail-value {{ color: #333; }}
        .payment-status {{ padding: 5px 10px; border-radius: 3px; color: white; font-weight: bold; }}
        .amount {{ font-size: 1.2em; font-weight: bold; color: #007bff; }}
        .footer {{ text-align: center; padding: 20px; color: #666; font-size: 0.9em; }}
        @media (max-width: 600px) {{
            .detail-row {{ flex-direction: column; }}
            .detail-label {{ margin-bottom: 5px; }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🏨 Booking Confirmation</h1>
            <p>Thank you for choosing our hotel!</p>
        </div>
        
        <div class='content'>
            <h2>Dear {billData.CustomrName},</h2>
            <p>We are pleased to confirm your reservation. Here are your booking details:</p>
            
            <div class='booking-details'>
                <div class='detail-row'>
                    <span class='detail-label'>Booking ID:</span>
                    <span class='detail-value'>#{billData.BookingID}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Guest Name:</span>
                    <span class='detail-value'>{billData.CustomrName}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Room Number:</span>
                    <span class='detail-value'>{billData.RoomNumber}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Check-in Date:</span>
                    <span class='detail-value'>{billData.CheckIn:dddd, MMMM dd, yyyy}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Check-out Date:</span>
                    <span class='detail-value'>{billData.CheckOut:dddd, MMMM dd, yyyy}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Total Nights:</span>
                    <span class='detail-value'>{totalNights} night{(totalNights > 1 ? "s" : "")}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Total Amount:</span>
                    <span class='detail-value amount'>${billData.Amount:F2}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Payment Status:</span>
                    <span class='payment-status' style='background-color: {paymentStatusColor};'>{paymentStatusText}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Booking Date:</span>
                    <span class='detail-value'>{billData.Created:MMMM dd, yyyy 'at' hh:mm tt}</span>
                </div>
            </div>
            
            <div style='margin-top: 20px; padding: 15px; background-color: #e7f3ff; border-left: 4px solid #007bff;'>
                <h3 style='margin-top: 0;'>Important Information:</h3>
                <ul>
                    <li>Check-in time: 3:00 PM</li>
                    <li>Check-out time: 11:00 AM</li>
                    <li>Please bring a valid ID for check-in</li>
                    <li>For any changes or cancellations, please contact us at least 24 hours in advance</li>
                </ul>
            </div>
        </div>
        
        <div class='footer'>
            <p>If you have any questions, please don't hesitate to contact us.</p>
            <p><strong>Hotel Booking System</strong><br>
            Phone: (555) 123-4567 | Email: reservations@hotel.com</p>
        </div>
    </div>
</body>
</html>";
        }

        private string GeneratePaymentConfirmationEmailBody(Bill_DTO billData)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Payment Confirmation</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #28a745; color: white; text-align: center; padding: 20px; border-radius: 5px 5px 0 0; }}
        .content {{ background-color: #f8f9fa; padding: 20px; border: 1px solid #dee2e6; }}
        .success-message {{ background-color: #d4edda; border: 1px solid #c3e6cb; color: #155724; padding: 15px; border-radius: 5px; text-align: center; margin: 20px 0; }}
        .payment-details {{ background-color: white; padding: 15px; margin: 15px 0; border-radius: 5px; border: 1px solid #dee2e6; }}
        .detail-row {{ display: flex; justify-content: space-between; margin: 10px 0; padding: 5px 0; border-bottom: 1px solid #eee; }}
        .detail-label {{ font-weight: bold; color: #555; }}
        .detail-value {{ color: #333; }}
        .amount {{ font-size: 1.2em; font-weight: bold; color: #28a745; }}
        .footer {{ text-align: center; padding: 20px; color: #666; font-size: 0.9em; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>✓ Payment Confirmed</h1>
            <p>Your payment has been processed successfully!</p>
        </div>
        
        <div class='content'>
            <div class='success-message'>
                <h2 style='margin: 0;'>🎉 Thank you for your payment!</h2>
                <p>Your reservation is now confirmed and fully paid.</p>
            </div>
            
            <h3>Dear {billData.CustomrName},</h3>
            <p>We have successfully received your payment for the following booking:</p>
            
            <div class='payment-details'>
                <div class='detail-row'>
                    <span class='detail-label'>Booking ID:</span>
                    <span class='detail-value'>#{billData.BookingID}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Room Number:</span>
                    <span class='detail-value'>{billData.RoomNumber}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Check-in:</span>
                    <span class='detail-value'>{billData.CheckIn:MMM dd, yyyy}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Check-out:</span>
                    <span class='detail-value'>{billData.CheckOut:MMM dd, yyyy}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Amount Paid:</span>
                    <span class='detail-value amount'>${billData.Amount:F2}</span>
                </div>
                <div class='detail-row'>
                    <span class='detail-label'>Payment Date:</span>
                    <span class='detail-value'>{DateTime.Now:MMMM dd, yyyy 'at' hh:mm tt}</span>
                </div>
            </div>
        </div>
        
        <div class='footer'>
            <p>Thank you for choosing our hotel!</p>
            <p><strong>Hotel Booking System</strong><br>
            Phone: (555) 123-4567 | Email: reservations@hotel.com</p>
        </div>
    </div>
</body>
</html>";

        }

    }
}
