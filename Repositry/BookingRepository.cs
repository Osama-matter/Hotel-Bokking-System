using Hotel_Bokking_System.Controllers;
using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Hotel_Bokking_System.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Bokking_System.Repositry
{
    public class BookingRepository : GenericRepository<Cls_Booking>, IBooking
    {

        Hotel_dbcontext dbcontext;
        IEmailService _emailService;
        public BookingRepository(Hotel_dbcontext context, IEmailService emailService) : base(context)
        {
            dbcontext = context;
            this._emailService=emailService;
        }

        public async Task<List<DTO_Booking>> GetAll()
        {
            var bookings = await dbcontext.Bookings
                .Include(e => e.Room)
                .Include(e => e.Customer)
                .ToListAsync();

            return bookings.Select(b => new DTO_Booking
            {
                BookingID = b.BookingID,
                CustomarID = b.CustomarID,
                RoomID =  b.RoomID,

                CustomrName = b.Customer.FullName,
                RoomNumber = b.Room.RoomNumber,

                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                Status = b.Status,
                Created = b.Created
            }).ToList();
        }

        public async Task<DTO_Booking?> GetById(int id)
        {
            var booking = await dbcontext.Bookings
                .Include(e => e.Room)
                .Include(e => e.Customer)
                .FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking == null) return null;

            return new DTO_Booking
            {
                BookingID = booking.BookingID,
                CustomarID = booking.CustomarID,
                RoomID = booking.RoomID,
                CustomrName = booking.Customer.FullName,
                RoomNumber = booking.Room.RoomNumber,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Status = booking.Status,
                Created = booking.Created
            };
        }





        public async Task<DTO_BookingDetails> GetBookingDetailsByIdAsync(int ID)
        {
            var booking = await dbcontext.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .Include(b => b.Payments) // لازم تكون ICollection<Cls_Payments>
                .FirstOrDefaultAsync(e => e.BookingID == ID);

            if (booking == null)
                return new DTO_BookingDetails();

            return new DTO_BookingDetails
            {
                BookingID = booking.BookingID,
                CustomerName = booking.Customer.FullName,
                CustomerPhone = booking.Customer.PhoneNumber,
                RoomNumber = booking.Room.RoomNumber,
                RoomType = booking.Room.Type.ToString(),
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                BookingStatus = booking.Status.ToString(),
                PaymentAmount = booking.Payments?.FirstOrDefault()?.Amount,  // أول عملية دفع فقط
                PaymentMethod = booking.Payments?.FirstOrDefault()?.Method.ToString(),
                PaymentStatus = booking.Payments?.FirstOrDefault()?.Status.ToString(),
                Created = booking.Created
            };
        }




        public async Task<int> Create(DTO_Booking dto)
        {
            // Validation
            await BookingValidator.ValidateAsync(dto, dbcontext);

            var booking = new Cls_Booking
            {
                CustomarID = dto.CustomarID,
                RoomID = dto.RoomID,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                Status = Cls_Booking.BookingStatus.Pending,
                Created = DateTime.Now,
            };

            dbcontext.Bookings.Add(booking);
            await dbcontext.SaveChangesAsync();

            var nights = (booking.CheckOut - booking.CheckIn).Days;
            if (nights <= 0)
                throw new InvalidOperationException("Check-out date must be after Check-in date.");
            
            var PriceperNight = dbcontext.Rooms
                .Where(c => c.RoomID == dto.RoomID) // حدد الـ CustomerID
                .Select(c => c.PricePerNight)
                .FirstOrDefault();

            var amount = PriceperNight * nights; 

            var customerEmail = dbcontext.Customers
                .Where(c => c.CustomarID == dto.CustomarID) // حدد الـ CustomerID
                .Select(c => c.Email)
                .FirstOrDefault();


            // Prepare Bill DTO for email
            var billDto = new Bill_DTO
            {
                BookingID = booking.BookingID,
                CustomrName = dto.CustomrName ?? "Valued Customer",
                RoomNumber = dto.RoomNumber ?? "N/A",
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Amount = amount,
                paymentStatus = Cls_Payments.PaymentStatus.Pending,
                Created = booking.Created
            };
 
            // Send Booking Confirmation Email
            if (!string.IsNullOrEmpty(customerEmail))
            {
                try
                {
                    await _emailService.SendBookingConfirmationEmailAsync(customerEmail, billDto);
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the booking
                    Console.WriteLine($"Failed to send booking email: {ex.Message}");
                }
            }

            return booking.BookingID;
        }

        public async Task<bool> Update(int id, DTO_Booking dto)
        {
            var booking = await dbcontext.Bookings.FindAsync(id);
            if (booking == null) return false;

            // Validation
            await BookingValidator.ValidateAsync(dto, dbcontext, id);

            booking.CustomarID = dto.CustomarID;
            booking.RoomID = dto.RoomID;
            booking.CheckIn = dto.CheckIn;
            booking.CheckOut = dto.CheckOut;
            booking.Status = dto.Status;
            booking.Created = dto.Created;

            dbcontext.Bookings.Update(booking);
            await   dbcontext.SaveChangesAsync();

            var nights = (booking.CheckOut - booking.CheckIn).Days;
            if (nights <= 0)
                throw new InvalidOperationException("Check-out date must be after Check-in date.");


            var PriceperNight = dbcontext.Rooms
                .Where(c => c.RoomID == dto.RoomID) // حدد الـ Roomid
                .Select(c => c.PricePerNight)
                .FirstOrDefault();

            var amount = PriceperNight * nights;
            var customerEmail = dbcontext.Customers
             .Where(c => c.CustomarID == dto.CustomarID) // حدد الـ CustomerID
             .Select(c => c.Email)
             .FirstOrDefault();



            // Optional: send email if payment status changed to Paid
            if (!string.IsNullOrEmpty(customerEmail) && booking.Status == Cls_Booking.BookingStatus.Pending)
            {
                var billDto = new Bill_DTO
                {
                    BookingID = booking.BookingID,
                    CustomrName = dto.CustomrName ?? "Valued Customer",
                    RoomNumber = dto.RoomNumber ?? "N/A",
                    CheckIn = booking.CheckIn,
                    CheckOut = booking.CheckOut,
                    Amount =  amount,
                    paymentStatus = Cls_Payments.PaymentStatus.Paid,
                    Created = booking.Created

                };

                try
                {
                    await _emailService.SendPaymentConfirmationEmailAsync(customerEmail, billDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send payment email: {ex.Message}");
                }
            }

            return true;
        }

        // ✅ Delete
        public async Task<bool> Delete(int id)
        {
            var booking = await dbcontext.Bookings.FindAsync(id);
            if (booking == null) return false;

            dbcontext.Bookings.Remove(booking);
            await dbcontext.SaveChangesAsync();

            return true;
        }

    }
}
