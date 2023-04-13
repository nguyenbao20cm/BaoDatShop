using BaoDatShop.DTO.Cart;
using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Security.Claims;
using BaoDatShop.DTO.InvoiceDetail;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = UserRole.Costumer)]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IProductTypeService productypeService;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly IInvoiceService invoiceService;
        private readonly IInvoiceDetailService invoiceDetailService;
        public CustomerController(
            IInvoiceDetailService invoiceDetailService,
            IInvoiceService invoiceService,
            IProductTypeService productypeService,
            IProductService productService,
            ICartService cartService
            )
        {
            this.invoiceService= invoiceService;
            this.invoiceDetailService=  invoiceDetailService;
            this.productypeService = productypeService;
            this.productService = productService;
            this.cartService = cartService;
        }

        //Invoice
        [HttpPost("CreateInvoice")]
        public async Task<IActionResult> CreateInvoice([FromForm] CreateInvoice model)
        {
            return Ok(invoiceService.Create(GetCorrectUserId(),model));
        }
        //Cart
        [HttpPost("CreateCart")]
        public async Task<IActionResult> CreateCart([FromForm] CreateCartNoAccId  model)
        {
         
            CreateCart result = new();
            result.ProductId = model.ProductId;
            result.AccountId = GetCorrectUserId();
            result.Quantity = model.Quantity;
            return Ok(cartService.Create(result));
        }
        [HttpGet("GetAllCart")]
        public async Task<IActionResult> GetAllCart()
        {
            return Ok(cartService.GetAll(GetCorrectUserId()));
        }
        [HttpPut("UpdateCart+1/{id}")]
        public async Task<IActionResult> UpdateCartUp1(int id)
        {
            return Ok(cartService.Up(id));
        }
        [HttpPut("UpdateCart-1/{id}")]
        public async Task<IActionResult> UpdateCartDown1(int id)
        {
            return Ok(cartService.Down(id));
        }
        [HttpDelete("DeleteCart/{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            return Ok(cartService.Delete(id));
        }
        [HttpDelete("DeleteAllCart")]
        public async Task<IActionResult> DeleteAllCart()
        {
            return Ok(cartService.DeleteAll(GetCorrectUserId()));
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}
