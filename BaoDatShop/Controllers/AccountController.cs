using BaoDatShop.DTO;
using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.LoginRequest;
using BaoDatShop.DTO.Response;
using BaoDatShop.DTO.Role;
using BaoDatShop.DTO.Voucher;
using BaoDatShop.Service;
using Eshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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
        private readonly IEmailSender IEmailSender;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> userManager;
        public AccountController(
            IAccountService accountService,
            IEmailSender IEmailSender,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            this.IEmailSender = IEmailSender;
            this.userManager = userManager;
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
            if (result == "Chưa xác minh Email") return Ok("Chưa xác minh Email");
            return Ok(result);
        }
        [HttpPost]
        [Route("register-Admin")]
        public async Task<ActionResult> RegisterAdmin([FromForm] RegisterRequest model)
        {
            var ba = _accountService.GetAllAccount();
            foreach (var item in ba)
            {

                if (model.Username == item.Username)
                    return Ok("User Name đã được sử dụng");
                if (model.Phone == item.Phone) return Ok("SDt da duoc su dung");
                if (model.Email == item.Email) return Ok("Email da duoc su dung");
            }
            var result = await _accountService.SignUpAdmin(model);
            if(result.Succeeded)
            {
                var  user = await userManager.FindByNameAsync(model.Username);
                if(user!=null)
                {
                
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string url = this.Url.ActionLink("ConfirmEmail", "Account",
                     new { token, email = model.Email });
                    SendVoucher a = new();
                    a.email = model.Email;
                    a.subject = "Xác minh tài khoản";
                    a.message = "Xác minh tài khoản bằng cách nhấn vào đường link:  <a href=\""
                                                       + url ;
                    IEmailSender.SendEmaiValidationEmail(a);
                }
            }
            if (result.Succeeded) 
                return Ok("Thanh cong");
            else
                return Ok(result.Errors);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var ba = _accountService.GetAllAccount().Where(a=>a.Email==email).FirstOrDefault();
            var user = await userManager.FindByIdAsync(ba.Id);
            if(user !=null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                    return Ok("Thành công");
            }
            return Ok("Người dùng không tồn tại");
        }
        [HttpPost]
        [Route("register-Customer")]
        public async Task<IActionResult> RegisterCustomer(ReuqestSignUp model)
        {
            var a = _accountService.GetAllAccount();
            foreach(var item in a)
            {
              
                if ( model.Username==item.Username)
                    return Ok(1);
                if (model.Phone == item.Phone) return Ok(2);
                if(model.Email==item.Email) return Ok(3);
            }
            var result = await _accountService.SignUpCustomer(model);
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string url = this.Url.ActionLink("ConfirmEmail", "Account",
                     new { token, email = model.Email });
                    SendVoucher a = new();
                    a.email = model.Email;
                    a.subject = "Xác minh tài khoản";
                    a.message = "Xác minh tài khoản bằng cách nhấn vào đường link:  <a href=\""
                                                       + url;
                    IEmailSender.SendEmaiValidationEmail(a);
                }
            }
            if (result.Succeeded)
                return Ok("Thanh cong");
            else
                return Ok(result.Errors);
           
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost]
        [Route("register-Staff")]
        public async Task<IActionResult> RegisterStaff(ReuqestSignUp model)
        {
            var result = await _accountService.RegisterStaff(model);
            if (result.Succeeded) return Ok(result.Succeeded);
            return Unauthorized();
        }
        [HttpPost("CreateAvatarImage")]
        public async Task<IActionResult> CreateAvatarImage( IFormFile model)
        {
            var a = _accountService.CreateAvatarImage(model);
            return Ok(a);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetQuantityAccount")]
        public async Task<IActionResult> GetQuantityAccount()
        {
            var a = _accountService.GetAllAccount().Where(a=>a.Permission==3).ToList();
            return Ok(a.Count);
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer)]
        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount( UpdateAccountRequest model)
        {
            var result = await _accountService.Update(GetCorrectUserId(), model);
            if (result == "Failed") return Ok("Failed");
            return Ok(result);
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer)]
        [HttpPut]
        [Route("UpdateAccountCustomer")]
        public async Task<IActionResult> UpdateAccountCustomer(UpdateAccountCustomerRequest model)
        {
            var result = await _accountService.UpdateAccountCustomer(GetCorrectUserId(), model);
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
        [Authorize(Roles =  UserRole.Admin)]
        [HttpGet]
        [Route("GetAllAccount")]
        public async Task<IActionResult> GetAllAccount()
        {
            var result = _accountService.GetAllAccount();
             return Ok(result);
           
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet]
        [Route("GetAllAccountStaff")]
        public async Task<IActionResult> GetAllAccountStaff()
        {
            var result = _accountService.GetAllAccount().Where(a=>a.Permission==2).ToList();
            return Ok(result);

        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut]
        [Route("DeleteAccount/{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var result = await _accountService.DeleteAccount(id);
            return Ok(result);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet]
        [Route("GetAllAcountCustomer")]
        public async Task<IActionResult> GetAllAcountCustomer()
        {
            var result = _accountService.GetAllAcountCustomer();
            return Ok(result);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet]
        [Route("GetAllAcountCustomerStatusTrue")]
        public async Task<IActionResult> GetAllAcountCustomerStatusTrue()
        {
            var result = _accountService.GetAllAcountCustomer().Where(a=>a.Status==true).ToList();
            return Ok(result);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet]
        [Route("GetAllAcountCustomerStatusFalse")]
        public async Task<IActionResult> GetAllAcountCustomerStatusFalse()
        {
            var result = _accountService.GetAllAcountCustomer().Where(a=>a.Status==false).ToList();
            return Ok(result);
        }
    
        private string GetCorrectUserId()
        {

            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }

    }
}
