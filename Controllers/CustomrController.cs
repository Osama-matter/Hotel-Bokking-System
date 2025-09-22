using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Employee")]
    public class CustomrController : ControllerBase
    {

        iCustomr CustomerRepo; 
        public  CustomrController (iCustomr _customerRepo)
        {
            CustomerRepo = _customerRepo;
        }

        [HttpGet("GetAlL")]
        public  async Task<IActionResult> GetAll()
        {
            var Customr =await CustomerRepo.GetAll();
            if(Customr != null)
            {
                return Ok(Customr);
            }
            return Ok("Not  Found nay Customr");
        }

        [HttpPost("CreateNew")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm]DTO_Customer Request_Customer)
        {
            if(ModelState.IsValid)
            {
                await CustomerRepo.Create(Request_Customer);
                return Ok("Add Scuess");
            }
            return BadRequest(); 
        }



        [HttpPut("{ID:int}")]
        [AllowAnonymous]

        public async Task<IActionResult> Edit(int  ID ,[FromForm] DTO_Customer Request_Customer)
        {
            if(ModelState.IsValid)
            {
                await CustomerRepo.Update( ID, Request_Customer);

                return Ok("Edit Sucess"); 
            }
            return BadRequest();
        }

        [HttpDelete("{ID:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int  ID)
        {
            if(await CustomerRepo.Delete(ID) == true)
            {
                return Ok("Deleted Sucess"); 
            }
            return BadRequest();
        }


         
    }
}
