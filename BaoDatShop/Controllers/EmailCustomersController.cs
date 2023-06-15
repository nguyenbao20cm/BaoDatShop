using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Mail;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailCustomersController : ControllerBase
    {
        private readonly AppDbContext context;
        public EmailCustomersController(AppDbContext context)
        {
            this.context = context;
        }
        private bool IsEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllEmailCustomer")]
        public async Task<IActionResult> GetAllEmailCustomer()
        {
            return Ok(context.EmailCustomer.ToList());
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpGet("CreateEmailCustomer")]
        public async Task<IActionResult> CreateEmailCustomer(string email)
        {
            if(IsEmail(email)==true)
            {
                EmailCustomer a = new();
                a.Name=email;
                context.Add(a);
                context.SaveChanges();
                return Ok("Succes");
            }
            return Ok("Failed");
        }
    }
}
