using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {

        IReview ReviewRepo; 

        public ReviewsController (IReview _review)
        {
            ReviewRepo = _review;
        }
        [HttpGet("ShowAll")]
        public async Task<IActionResult> ShowAll()
        {
            var reviews = await ReviewRepo.ShowAll(); // لازم تعمل method في Repo
            return Ok(reviews);
        }



        [HttpPost("Create")]
        public async Task<IActionResult>Create([FromForm]DTO_Reviews _Reviews)
        {
            if(ModelState.IsValid)
            {
                await ReviewRepo.Create(_Reviews);
                return Ok("Create Secuss"); 
            }
            return BadRequest(ModelState); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int  id  , [FromForm]DTO_Reviews _Reviews)
        {
            await ReviewRepo.Update(id, _Reviews);
            return Ok(); 
        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete(int  id  )
        {
            if( await  ReviewRepo.Delete(id)   == true )
            {
                return Ok("Deleted  seuess"); 
            }
            return BadRequest("Not Found  ");
        }

    }
}
