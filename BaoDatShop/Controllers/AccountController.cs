using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.LoginRequest;
using BaoDatShop.DTO.Response;
using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        
        private readonly IConfiguration _configuration;
        public AccountController(
            IAccountService accountService,
        
           IConfiguration configuration)
        {
          
            _configuration = configuration;
            _accountService = accountService;
        }
        //[HttpPost("SignupUser")]
        //public async Task<IActionResult> signup( RegisterRequest model)
        //{
        //    var result = await _accountService.SignUp(model);
        //    if (result.Succeeded) return Ok(result.Succeeded);
        //    return Unauthorized(result);
        //}
        [HttpPost("Signin")]
        public async Task<IActionResult> signin(LoginRequest model)
        {  
            if(model== null) return Ok("Failed");
            await _accountService.SignIn(model);
            var result = await _accountService.SignIn(model);
            if (result== "Failed") return Ok("Failed");
            return Ok(result);
        }
        [HttpPost]
        [Route("register-Admin")]
        public async Task<IActionResult> RegisterAdmin([FromForm] RegisterRequest model)
        {
            var result = await _accountService.SignUpAdmin(model);
            if (result.Succeeded) return Ok(result.Succeeded);
            return Unauthorized();
        }
        [HttpPost]
        [Route("register-Customer")]
        public async Task<IActionResult> RegisterCustomer([FromForm] RegisterRequest model)
        {
            var result = await _accountService.SignUpCustomer(model);
            if (result.Succeeded) return Ok(result.Succeeded);
            return Unauthorized();
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer)]
        [HttpPost]
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromForm] UpdateAccountRequest model)
        {
            var result = await _accountService.Update(GetCorrectUserId(), model);
            if (result == "Failed") return Ok("Failed");
            return Ok(result);
        }
        [HttpGet]
        [Route("GetDetailAccount")]
        public async Task<IActionResult> GetDetailAccount()
        {
            var result =  _accountService.GetDetailAccount(GetCorrectUserId());
            if (result!=null) return Ok(result);
            return Unauthorized();
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }

    }
}
