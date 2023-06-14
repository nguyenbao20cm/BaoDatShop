using BaoDatShop.DTO.Cart;
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
    [Authorize(Roles = UserRole.Costumer)]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService cartService;
        public CartsController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost("CreateCart")]
        public async Task<IActionResult> CreateCart(CreateCartNoAccId model)
        {

            CreateCartRequest result = new();
            result.ProductSizeId = model.ProductSizeId;
            result.AccountId = GetCorrectUserId();
            result.Quantity = model.Quantity;
            return Ok(cartService.Create(result));
        }
        [HttpGet("GetAllCart")]
        public async Task<IActionResult> GetAllCart()
        {
            return Ok(cartService.GetAll(GetCorrectUserId()));
        }
        [HttpGet("GetAllTotal")]
        public async Task<IActionResult> GetAllTotal()
        {
            return Ok(cartService.GetAllTotal(GetCorrectUserId()));
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
