﻿using BaoDatShop.DTO.AccountRequest;
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
        private readonly IEmailSender IEmailSender;
        private readonly IConfiguration _configuration;
        public AccountController(
            IAccountService accountService,
        IEmailSender IEmailSender,
           IConfiguration configuration)
        {
          this.IEmailSender = IEmailSender;
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
        [HttpPost("SendVoucher")]
        public async Task<IActionResult> Index(string email, string subject, string message)
        {
             IEmailSender.SendEmailAsync(email, subject, message);
            return Ok("ok");
        }
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
        public async Task<IActionResult> RegisterCustomer(ReuqestSignUp model)
        {
            var a = _accountService.GetAllAccount();
            foreach(var item in a)
            {
              
                if ( model.Username==item.Username)
                    return Ok("Tên đăng nhập này đã được sử dụng");
                if (model.Phone == item.Phone) return Ok("Số điện thoại này đã được sử dụng");
            }
            var result = await _accountService.SignUpCustomer(model);
            if (result.Succeeded) return Ok(result.Succeeded);
            return Unauthorized();
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
