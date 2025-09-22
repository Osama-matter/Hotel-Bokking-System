using Hotel_Bokking_System.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> SaveRole(DTO_Role viewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = viewModel.Name
                };

                IdentityResult result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {

                    return Ok("Create Sucess"); 
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return BadRequest( ModelState);
        }

    }
}
