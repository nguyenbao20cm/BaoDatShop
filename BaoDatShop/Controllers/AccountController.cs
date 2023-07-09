using BaoDatShop.DTO;
using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.LoginRequest;
using BaoDatShop.DTO.Response;
using BaoDatShop.DTO.Role;
using BaoDatShop.DTO.Voucher;
using BaoDatShop.Model.Context;
using BaoDatShop.Service;
using Eshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
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
        private readonly AppDbContext context;
        private readonly IAccountService _accountService;
        private readonly IEmailSender IEmailSender;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> userManager;
      
        public AccountController(
            AppDbContext context,
            IAccountService accountService,
            IEmailSender IEmailSender,
            UserManager<ApplicationUser> userManager,
             IWebHostEnvironment _environment,
            IConfiguration configuration)
        {
            this._environment = _environment;
            this.context = context;
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

   
        [HttpGet("GetAllAccountCustomerStatusFalse")]
        public async Task<IActionResult> GetAllAccountCustomerStatusFalse()
        {
            return Ok(context.Account.Where(a=>a.Status==false).Where(a=>a.Permission==3).ToList());
        }
        [HttpGet("GetAllAccountCustomerStatusTrue")]
        public async Task<IActionResult> GetAllAccountCustomerStatusTrue()
        {
            return Ok(context.Account.Where(a => a.Status == true).Where(a => a.Permission == 3).ToList());
        }
        [HttpGet("GetAllAccountStaffStatusFalse")]
        public async Task<IActionResult> GetAllAccountStaffStatusFalse()
        {
            return Ok(context.Account.Where(a => a.Status == false).Where(a => a.Permission == 2).ToList());
        }
        [HttpGet("GetAllAccountStaffStatusTrue")]
        public async Task<IActionResult> GetAllAccountStaffStatusTrue()
        {
            return Ok(context.Account.Where(a => a.Status == true).Where(a => a.Permission == 2).ToList());
        }
        [HttpPost("ResetPassWord")]
        public async Task<IActionResult> ChangePassWordForgot(ResetPassWordWithToken model)
        {

            if (model.Password != model.ConfirmPassword) return Ok("Mật khẩu xác thực không khớp");
            var abbb = _accountService.GetAllAccount();
         
            var ba = _accountService.GetAllAccount().Where(a => a.Email == model.Email).FirstOrDefault();
            var user = await userManager.FindByIdAsync(ba.Id);
            if (user != null)
            {
                var a = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if(a.Succeeded)
                return Ok("Thành công");
            }
            return Ok("Thất bại");
        }
        [HttpPost("Signin")]
        public async Task<IActionResult> signin(LoginRequest model)
        {
            if (model == null) return Ok("Failed");
            await _accountService.SignIn(model);
            var result = await _accountService.SignIn(model);
            if (result == "Failed") return Ok("Failed");
            if (result == "Chưa xác minh Email") return Ok("Chưa xác minh Email");
            return Ok(result);
        }
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> ForgetPassword(ForgotPass email)
        {
            var ua = this.Url;
            var ba = _accountService.GetAllAccount().Where(a => a.Email == email.Email).FirstOrDefault();
            if(ba==null) return Ok("Email này không khớp với tài khoản nào cả");
            var user = await userManager.FindByIdAsync(ba.Id);
            if (user.EmailConfirmed == false) return Ok("Thất bại, vì tài khoản này chưa được kích hoạt");
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                //string url = this.Url.ActionLink("TokenForgotPass", "Account",
                //   new { token, email = email });
                //cua tao gia bao
                string url = "http://localhost:3000/auth/DoiMatKhau?Token=" + token + "&Email=" + email.Email;

                //cua tao Dat
                //string url = "http://localhost:3000/auth/DoiMatKhau?Token=" + token + "&Email=" + email.Email;
                SendVoucher a = new();
                a.email = email.Email;
                a.subject = "Quên mật khẩu";
                a.message = "Đổi mật khẩu bằng cách nhấn vào đường link:  <a href=\""
                                                                     + url + "\">link</a>";
                IEmailSender.SendEmaiValidationEmail(a);
                return Ok("Thành công");
            }
            return Ok("Thất bại");
        }
        [HttpPost("ForgotPasswordCustomer")]
        public async Task<ActionResult> ForgotPasswordCustomer(ForgotPass email)
        {
            var ua = this.Url;
            var ba = _accountService.GetAllAccount().Where(a => a.Email == email.Email).FirstOrDefault();
            if (ba == null) return Ok("Email này không khớp với tài khoản nào cả");
            var user = await userManager.FindByIdAsync(ba.Id);
            if (user.EmailConfirmed == false) return Ok("Thất bại, vì tài khoản này chưa được kích hoạt");
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                //string url = this.Url.ActionLink("TokenForgotPass", "Account",
                //   new { token, email = email });
                // cua gia bao
               // string url = "http://localhost:3000/auth/DoiMatKhau?Token=" + token + "&Email=" + email.Email;


                //cua tao Dat
                string url = "http://localhost:3000/DoiMatKhau?Token=" + token + "&Email=" + email.Email;
                SendVoucher a = new();
                a.email = email.Email;
                a.subject = "Quên mật khẩu";
                a.message = "Đổi mật khẩu bằng cách nhấn vào đường link:  <a href=\""
                                                                     + url + "\">link</a>";
                IEmailSender.SendEmaiValidationEmail(a);
                return Ok("Thành công");
            }
            return Ok("Thất bại");
        }
        [HttpGet("TokenForgotPass")]
        public async Task<IActionResult> TokenForgotPass(TokenResetPassword model)
        {
            return Ok(new { Email = model.Email, Token = model.Token  });
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
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user != null)
                {

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //string url = this.Url.ActionLink("ConfirmEmail", "Account",
                    // new { token, email = model.Email });
                    //cua gia bao
                    string url = "http://localhost:3000/auth/DangNhap?Token=" + token + "&Email=" + model.Email;

                    //cua tao Dat
                    //string url = "http://localhost:3000/auth/DoiMatKhau?Token=" + token + "&Email=" + email.Email;
                    SendVoucher a = new();
                    a.email = model.Email;
                    a.subject = "Xác minh tài khoản";
                    a.message = "Xác minh tài khoản bằng cách nhấn vào đường link:  <a href=\""
                                                                      + url + "\">link</a>";
                    IEmailSender.SendEmaiValidationEmail(a);
                }
            }
            if (result.Succeeded)
                return Ok("Thanh cong");
            else
                return Ok(result.Errors);
        }
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(TokenResetPassword model)
        {
            var ba = _accountService.GetAllAccount().Where(a => a.Email == model.Email).FirstOrDefault();
            var user = await userManager.FindByIdAsync(ba.Id);
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, model.Token);
                if (result.Succeeded)
                    return Ok("Thành công");
            }
            return Ok("Người dùng không tồn tại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("ActiveAccount/{id}")]
        public async Task<IActionResult> ActiveAccount(string id)
        {
            var check= await _accountService.ActiveAccount(GetCorrectUserId(),id);
            if(check==true)
            {
                return Ok(true);
            }
            else return Ok(false);
        }

        [HttpPost]
        [Route("register-Customer")]
        public async Task<IActionResult> RegisterCustomer(ReuqestSignUp model)
        {
            var ab = _accountService.GetAllAccount();
            foreach (var item in ab)
            {

                if (model.Username == item.Username)
                    return Ok(1);
                if (model.Phone == item.Phone) return Ok(2);
                if (model.Email == item.Email) return Ok(3);
            }
            var result = await _accountService.SignUpCustomer(model);
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //string url = this.Url.ActionLink("ConfirmEmail", "Account",
                    // new { token, email = model.Email });
                    //dat 3001
                    string url = " http://localhost:3000/login?Token=" + token + "&Email=" + model.Email;
                    SendVoucher a = new();
                    a.email = model.Email;
                    a.subject = "Xác minh tài khoản";

                    a.message = "Xác minh tài khoản bằng cách nhấn vào đường link:  <a href=\""
                                                                       + url + "\">link</a>";
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
            var ab = _accountService.GetAllAccount();
            foreach (var item in ab)
            {
                if (model.Username == item.Username)
                    return Ok("12");
                if (model.Phone == item.Phone) return Ok(2);
                if (model.Email == item.Email) return Ok(3);
            }
            var result = await _accountService.RegisterStaff(model);
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.Username);
                if (user != null)
                {

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //string url = this.Url.ActionLink("ConfirmEmail", "Account",
                    // new { token, email = model.Email });
                    //cua gia bao
                    string url = "http://localhost:3000/auth/DangNhap?Token=" + token + "&Email=" + model.Email;

                    //cua tao Dat
                    //string url = "http://localhost:3000/auth/DoiMatKhau?Token=" + token + "&Email=" + email.Email;
                    SendVoucher a = new();
                    a.email = model.Email;
                    a.subject = "Xác minh tài khoản";
                    a.message = "Xác minh tài khoản bằng cách nhấn vào đường link:  <a href=\""
                                                                      + url + "\">link</a>";
                    IEmailSender.SendEmaiValidationEmail(a);
                }
            }
            if (result.Succeeded) return Ok(result.Succeeded);
            return Unauthorized();
        }
        [HttpPost("CreateAvatarImage")]
        public async Task<IActionResult> CreateAvatarImage(IFormFile model)
        {
            var a = _accountService.CreateAvatarImage(model);
            return Ok(a);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetQuantityAccount")]
        public async Task<IActionResult> GetQuantityAccount()
        {
            var a = _accountService.GetAllAccount().Where(a => a.Permission == 3).ToList();
            return Ok(a.Count);
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer)]
        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(UpdateAccountRequest model)
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

        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer)]
        [HttpPost]
        [Route("UpdateAccountCustomer/{Phone}&{Address}&{FullName}")]
        public async Task<IActionResult> UpdateAccountWithImage(string Phone,string Address,string FullName,  IFormFile model)
        {

            var a = context.Account.Where(a => a.Id == GetCorrectUserId()).FirstOrDefault();
            if (Phone != a.Phone)
            {
                foreach (var abc in context.Account)
                {
                    if (abc.Phone == Phone) return Ok("SDT");// SDT bij trungf
                }
            }
            a.Phone = Phone;
            a.Address = Address;
            a.FullName = FullName;
            a.Avatar = a.Id+".jpg";
            context.Update(a);
            var check1 = context.SaveChanges();
            if(check1 == 0) return Ok("Thất bại");
            var user = await userManager.FindByIdAsync(GetCorrectUserId());
            user.Phone = Phone;
            user.Address = Address;
            user.Avatar = user.Id + ".jpg";
            user.FullName = FullName;
            var check= await userManager.UpdateAsync(user);
            if(check.Succeeded)
            {
                var fileName = a.Id + ".jpg";
                var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Avatar");
                var uploadPath = Path.Combine(uploadFolder, fileName);
                using (FileStream fs = System.IO.File.Create(uploadPath))
                {
                    model.CopyTo(fs);
                    fs.Flush();
                }
                return Ok(true);
            }
            else
                return Ok("Thất bại");


        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer)]
        [HttpGet]
        [Route("GetDetailAccount")]
        public async Task<IActionResult> GetDetailAccount()
        {
            var result = _accountService.GetDetailAccount(GetCorrectUserId());
            if (result != null) return Ok(result);
            return Unauthorized();
        }
        [Authorize(Roles = UserRole.Admin)]
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
            var result = _accountService.GetAllAccount().Where(a => a.Permission == 2).ToList();
            return Ok(result);

        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut]
        [Route("DeleteAccount/{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var result = await _accountService.DeleteAccount(GetCorrectUserId(),id);
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
            var result = _accountService.GetAllAcountCustomer().Where(a => a.Status == true).ToList();
            return Ok(result);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet]
        [Route("GetAllAcountCustomerStatusFalse")]
        public async Task<IActionResult> GetAllAcountCustomerStatusFalse()
        {
            var result = _accountService.GetAllAcountCustomer().Where(a => a.Status == false).ToList();
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




