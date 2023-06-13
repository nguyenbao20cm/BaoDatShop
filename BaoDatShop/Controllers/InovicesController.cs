using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
   
    [ApiController]
    public class InovicesController : ControllerBase
    {
        private readonly IInvoiceService invoiceService;
        public InovicesController(IInvoiceService invoiceService)
        {
            this.invoiceService = invoiceService;
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("CreateInvoice")]
        public async Task<IActionResult> CreateInvoice(CreateInvoiceRequest model)
        {
            return Ok(invoiceService.Create(GetCorrectUserId(), model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInovice")]
        public async Task<IActionResult> GetAllInovice()
        {
            return Ok(invoiceService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInoviceTotalMonth/{year}")]
        public async Task<IActionResult> GetAllInoviceTotalMonth(string year)
        {
            return Ok(invoiceService.GetAllInoviceTotalMonth(year));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateInovice/{id}")]
        public async Task<IActionResult> UpdateInovice(int id, UpdateInvoice model)
        {
            if (model.pay == null) return Ok("Thất bại");
            if (model.orderStatus == null) return Ok("Thất bại");
            if (model.shippingadress == null) return Ok("Thất bại");
            if (model.shippingphone == null) return Ok("Thất bại");
            if (invoiceService.UpdateInovice(id, model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        
        [HttpGet("GetAllInoviceFilterByDate/{startDate},{endDate}")]
        public async Task<IActionResult> GetAllInoviceFilterByDate(string startDate,string endDate)
        {
            return Ok(invoiceService.GetAllInoviceFilterByDate(startDate, endDate));
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}
