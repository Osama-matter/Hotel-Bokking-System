using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.DTO;
using Microsoft.EntityFrameworkCore;

public static class BookingValidator
{
    public static async Task ValidateAsync(DTO_Booking dto, Hotel_dbcontext dbcontext, int? bookingId = null)
    {
        // Check-In not in the past
        if (dto.CheckIn < DateTime.Today)
            throw new Exception("Check-in date cannot be in the past.");

        // Check-In before Check-Out
        if (dto.CheckIn >= dto.CheckOut)
            throw new Exception("Check-in date must be earlier than check-out date.");

        // Valid Customer
        if (dto.CustomarID <= 0)
            throw new Exception("Invalid Customer ID.");

        // Room exists
        var roomExists = await dbcontext.Rooms.AnyAsync(r => r.RoomID == dto.RoomID);
        if (!roomExists)
            throw new Exception("Room not found.");

        // Room availability
        bool isRoomAvailable = await IsRoomAvailableAsync(dbcontext, dto.RoomID, dto.CheckIn, dto.CheckOut, bookingId);
        if (!isRoomAvailable)
            throw new Exception("Room is already booked for the selected dates.");
    }

    private static async Task<bool> IsRoomAvailableAsync(
        Hotel_dbcontext dbcontext,
        int roomId,
        DateTime checkIn,
        DateTime checkOut,
        int? excludeBookingId = null)
    {
        return !await dbcontext.Bookings
            .AnyAsync(b =>
                b.RoomID == roomId &&
                (excludeBookingId == null || b.BookingID != excludeBookingId) && // ✅ استثناء الحجز الحالي
                (
                    (checkIn >= b.CheckIn && checkIn < b.CheckOut) ||
                    (checkOut > b.CheckIn && checkOut <= b.CheckOut) ||
                    (checkIn <= b.CheckIn && checkOut >= b.CheckOut)
                )
            );
    }
}
