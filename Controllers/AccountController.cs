using Hotel_Bokking_System.DTO;
using Hotel_Bokking_System.UserApplection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hotel_Bokking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;    // Add  user  manger  service to use  it to Make opperation

        IConfiguration _configuration;
        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {

            _userManager = userManager;    // declerate  usemanger  service  ; 
            _configuration = configuration;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromForm]DTO_Register regiestermodel)    /// Make  Post Action to Make  Register Servies 
        {
            if (ModelState.IsValid)  // Check Validation of Reguster  data   . 
            {
                ApplicationUser user = new ApplicationUser();   // declerate  object from FormApplection to updata  data from form to save reuister 
                user.UserName = regiestermodel.UserName;    // set  data in Applection Model 
                user.PhoneNumber = regiestermodel.Phone;   // set  Phone  
                user.Email = regiestermodel.Email;  // set  Eamil ; 
                user.Address= regiestermodel.Address;

                IdentityResult result = await _userManager.CreateAsync(user, regiestermodel.Password);

                if (result.Succeeded)   // Check Succeded of  Save  Reguister  data  
                {
                    await _userManager.AddToRoleAsync(user, "guest");
                    return Ok("Create Succes");    // return Stuts  code  200 to show data save succed
                }

                else
                {
                    foreach (var item in result.Errors)     //   Show eroor  if  found 
                    {
                        ModelState.AddModelError("Password", item.Description);
                    }
                }

            }


            return BadRequest(ModelState);

        }

        [HttpPost("addAdmin")]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdmin([FromForm] DTO_Register regiestermodel)    /// Make  Post Action to Make  Register Servies 
        {
            if (ModelState.IsValid)  // Check Validation of Reguster  data   . 
            {
                ApplicationUser user = new ApplicationUser();   // declerate  object from FormApplection to updata  data from form to save reuister 
                user.UserName = regiestermodel.UserName;    // set  data in Applection Model 
                user.PhoneNumber = regiestermodel.Phone;   // set  Phone  
                user.Email = regiestermodel.Email;  // set  Eamil ; 
                user.Address= regiestermodel.Address;

                IdentityResult result = await _userManager.CreateAsync(user, regiestermodel.Password);

                if (result.Succeeded)   // Check Succeded of  Save  Reguister  data  
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    return Ok("Create Succes");    // return Stuts  code  200 to show data save succed
                }

                else
                {
                    foreach (var item in result.Errors)     //   Show eroor  if  found 
                    {
                        ModelState.AddModelError("Password", item.Description);
                    }
                }

            }


            return BadRequest(ModelState);

        }


        [HttpPost("addEmployee")]

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromForm] DTO_Register regiestermodel)    /// Make  Post Action to Make  Register Servies 
        {
            if (ModelState.IsValid)  // Check Validation of Reguster  data   . 
            {
                ApplicationUser user = new ApplicationUser();   // declerate  object from FormApplection to updata  data from form to save reuister 
                user.UserName = regiestermodel.UserName;    // set  data in Applection Model 
                user.PhoneNumber = regiestermodel.Phone;   // set  Phone  
                user.Email = regiestermodel.Email;  // set  Eamil ; 
                user.Address= regiestermodel.Address;

                IdentityResult result = await _userManager.CreateAsync(user, regiestermodel.Password);

                if (result.Succeeded)   // Check Succeded of  Save  Reguister  data  
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return Ok("Create Succes");    // return Stuts  code  200 to show data save succed
                }

                else
                {
                    foreach (var item in result.Errors)     //   Show eroor  if  found 
                    {
                        ModelState.AddModelError("Password", item.Description);
                    }
                }

            }


            return BadRequest(ModelState);

        }




        [HttpPost("Login")]

        public async Task<IActionResult> Login( [FromForm]DTOLogin loginModel)   //Make  Login Action take  LoginDTO
        {
            if (ModelState.IsValid)   // Check Validation OfData 
            {
                ApplicationUser User =
                   await _userManager.FindByNameAsync(loginModel.UserName);    // Generate Applection user  to Get user  have  same  UserName   to Use  it  to check Password 

                if (User != null)   // Check Object  Not  == Null Mene  User Found 
                {
                    bool Found = await _userManager.CheckPasswordAsync(User, loginModel.Password);    // Make  Bool Var  to Check If  password  Valid  or not  valid 
                    if (Found == true)   // if  Usee Name fpund  and  Password  Is Valid  Generate Token
                    {
                        /////    Generate Token  //////


                        ///// Make Clims  ///
                        List<Claim> User_claims = new List<Claim>();
                        User_claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));// key 
                        User_claims.Add(new Claim(ClaimTypes.NameIdentifier, User.Id)); // put  ID  
                        User_claims.Add(new Claim(ClaimTypes.Name, User.UserName));
                        var UserRole = await _userManager.GetRolesAsync(User);  // Make  var to select  all role  
                        foreach (var Role in UserRole)  // uploud role  in Claims
                        {
                            User_claims.Add(new Claim(ClaimTypes.Role, Role));    // add  role  in Claims 
                        }
                        var SignInKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));  // Make  sinin key  make  Cradintal Valid  

                        SigningCredentials credentials = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);   // Make  Cradentials By key  and  Algorizem Hashing  


                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: _configuration["Jwt:Issuer"],
                            audience: _configuration["Jwt:Audience"],
                            signingCredentials: credentials,
                            claims: User_claims,
                            expires: DateTime.UtcNow.AddHours(1)
                            );



                        /// Generate  Toke  Response  ////

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),

                            expiration = DateTime.UtcNow.AddHours(1)  //OR//token Validto   

                        });  // return token from user 


                    }
                }
                ModelState.AddModelError("UserName", "UserName OR Password  UNVaild");    // retrun this  if  at  leat one  == null 


            }
            return BadRequest(ModelState);   // if  Modestate not  valid  

        }



    }
}
