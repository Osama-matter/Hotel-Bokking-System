using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")] // أي API هنا محتاج JWT
    public class RoomsController : ControllerBase
    {
        private readonly IRoom RoomRepo;

        public RoomsController(IRoom roomRepo)
        {
            RoomRepo = roomRepo;
        }

        // GET: api/rooms/all
        [HttpGet("all")]
        [AllowAnonymous]

        public async Task<IActionResult> GetAll()
        {
            var rooms = await RoomRepo.ShowRooms();
            return Ok(rooms);
        }

        // GET: api/rooms/find/5
        [HttpGet("find/{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var room = await RoomRepo.ShowRoomById(id);
            if (room == null)
                return NotFound($"Room details for ID {id} not found.");

            return Ok(room);
        }

        // POST: api/rooms/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] DTO_CreateRoom roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await RoomRepo.CreateRoom(roomDto);
            return Ok("Room saved successfully.");
        }

        // PUT: api/rooms/edit/5
        [HttpPut("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, DTO_CreateRoom editDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedRoom = await RoomRepo.Edit(id, editDto);

            if (updatedRoom == null)
                return NotFound($"Room with ID {id} not found.");

            return Ok(updatedRoom);
        }

        // DELETE: api/rooms/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await RoomRepo.DeleteAsync(id))
                return Ok();

            return NotFound();
        }

        // GET: api/rooms/details/5
        [HttpGet("details/{id:int}")]
        [AllowAnonymous]

        public async Task<IActionResult> ShowDetails(int id)
        {
            var details = await RoomRepo.ShowRoomDetiles(id);
            if (details == null)
                return NotFound($"Room details for ID {id} not found.");

            return Ok(details);
        }

        // GET: api/rooms/filter
        [HttpGet("filter")]
        [AllowAnonymous]

        public async Task<IActionResult> FilterRoom(
            Cls_Room.RoomStatus? status,
            Cls_Room.BedType? bedType,
            int? floor,
            Cls_Room.RoomType? roomType,
            decimal? pricePerNight)
        {
            var rooms = await RoomRepo.FindUsingData(status, bedType, floor, roomType, pricePerNight);

            if (rooms == null)
                return NotFound("No rooms match the given criteria.");

            return Ok(rooms);
        }
    }
}
