using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_Bokking_System.Models
{
    public class Cls_Payments
    {


        public enum PaymentMethod
        {
            Cash,
            CreditCard,
            DebitCard,
            BankTransfer,
            PayPal
        }

        public enum PaymentStatus
        {
            Pending,
            Processing,
            Paid,
            Failed,
            Refunded,
            Cancelled
        }

        [Key]
        public int PaymentID { get; set; }

        [Required]
        public int BookingID { get; set; }

        [ForeignKey(nameof(BookingID))]
        public Cls_Booking Booking { get; set; }   // ⬅️ هنا العلاقة 1:1 أو 1:Many

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public PaymentMethod Method { get; set; }

        [Required]
        [Display(Name = "Payment Status")]
        public PaymentStatus Status { get; set; }
    }
}
