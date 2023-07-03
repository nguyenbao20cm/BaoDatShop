using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class InvoiceDetailsController : ControllerBase
    {
        private readonly IInvoiceDetailService invoiceDetailService;
        public InvoiceDetailsController(IInvoiceDetailService invoiceDetailService)
        {
            this.invoiceDetailService = invoiceDetailService;
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer)]
        [HttpGet("GetAllInvoiceDetails/{id}")]
        public async Task<IActionResult> GetAllNewDetail(int id)
        { 
            return Ok(invoiceDetailService.GetAll(id));
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer)]
        [HttpGet("GetAllInvoiceDetails")]
        public async Task<IActionResult> GetAllInvoiceDetails()
        {
            return Ok(invoiceDetailService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer)]
        [HttpGet("GetQualityProductSell")]
        public async Task<IActionResult> GetQualityProductSell()
        {
            var a = invoiceDetailService.GetAll();
            var tamp = 0;
            foreach(var item in a)
            {
                tamp += item.Quantity;
            }    
            return Ok(tamp);
        }
    }
}
