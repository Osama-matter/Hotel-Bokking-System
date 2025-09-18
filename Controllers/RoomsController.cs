using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {

        IRoom RoomRepo; 

        public  RoomsController(IRoom roomRepo)
        {
            RoomRepo = roomRepo;
        }

        [HttpGet("Get All Room")]

        public async Task<IActionResult> GetAll()
        {
            // استدعاء البيانات من الـ Repo
            var Room = await RoomRepo.ShowRooms();


            return Ok(Room);   // return Data 
        }

        [HttpGet("Find Using ID {ID}")]

        public async Task<IActionResult> GetByID(int  ID)
        {
            // استدعاء البيانات من الـ Repo
           var  Room = await RoomRepo.ShowRoomById(ID);
           if(Room == null)
           {
                return BadRequest($"Room details for ID {ID} not found.");
           }

            return Ok(Room);   // return Data 
        }
        [HttpPost("Create")]

        public async Task<IActionResult> Create([FromForm]DTO_CreateRoom _Rooms)
        {

            if(ModelState.IsValid)
            {
                await RoomRepo.CreateRoom(_Rooms); 
                return Ok("Saved  OK");
            }
            return BadRequest("Feild  to save  data  ");

        }

        [HttpPut("Edit/{ID}")]
        public async Task<IActionResult> Edit(int ID, DTO_CreateRoom dTO_EditRoom)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedRoom = await RoomRepo.Edit(ID, dTO_EditRoom);

            if (updatedRoom == null)
                return NotFound($"Room with ID {ID} not found.");

            return Ok(updatedRoom);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await RoomRepo.DeleteAsync(id) == true )
            {
              
                return Ok();
            }
            return NotFound();
        }

        // GET: api/rooms/details/5
        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> ShowDetiles(int id)
        {
            var details = await RoomRepo.ShowRoomDetiles(id);
            if (details == null)
                return NotFound($"Room details for ID {id} not found.");

            return Ok(details);
        }


        [HttpGet("filter")]
        public async Task<IActionResult> FiltrRoom(Cls_Room.RoomStatus? status, Cls_Room.BedType? bedType ,int ? Floor , Cls_Room.RoomType? roomType, decimal? priceperNight)
        {
            var Room =await RoomRepo.FindUsingData(status , bedType , Floor , roomType , priceperNight);
            if(Room == null)
            {
                return BadRequest($"Not Found room have status {status} "); 
            }
            return Ok(Room);
        }

    }
}
