using Hotel_Bokking_System.Data;
using System.ComponentModel.DataAnnotations;

public class UniqueRoomNumberAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        var dbContext = (Hotel_dbcontext)validationContext.GetService(typeof(Hotel_dbcontext));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        if (value != null)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string roomNumber = value.ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            bool exists = dbContext.Rooms.Any(r => r.RoomNumber == roomNumber);   // check Condation if  found  room number  have  same  number  ; 

            if (exists)
                return new ValidationResult("Room number must be unique.");
        }

        return ValidationResult.Success;
    }
}
