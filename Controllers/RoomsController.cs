using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
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

        [HttpGet(Name = "Get All Room")]

        public async Task<IActionResult> GetAll()
        {
            // استدعاء البيانات من الـ Repo
            var Room = await RoomRepo.ShowRooms();


            return Ok(Room);   // return Data 
        }

        [HttpGet("{ID}")]

        public async Task<IActionResult> GetByID(int  ID)
        {
            // استدعاء البيانات من الـ Repo
           var  Room = await RoomRepo.ShowRoomById(ID);
           if(Room == null)
           {
                return BadRequest(); 
           }

            return Ok(Room);   // return Data 
        }
        [HttpPost]

        public async Task<IActionResult> Create([FromForm]DTO_CreateRoom _Rooms)
        {

            if(ModelState.IsValid)
            {
                await RoomRepo.CreateRoom(_Rooms); 
                return Ok();
            }
            return BadRequest();

        }

        [HttpPut("{ID:int}")]
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
    }
}
