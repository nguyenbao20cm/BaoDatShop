using BaoDatShop.DTO.CreateFavoriteProduct;
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
    public class FavoriteProductsController : ControllerBase
    {
        private readonly IFavoriteProductService IFavoriteProductService;
        public FavoriteProductsController(IFavoriteProductService IFavoriteProductService)
        {
            this.IFavoriteProductService = IFavoriteProductService;
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpGet("GetAllFavoriteProduct")]
        public async Task<IActionResult> FavoriteProduct()
        {
            return Ok(IFavoriteProductService.GetAll(GetCorrectUserId()));
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpGet("DeleteFavoriteProduct")]
        public async Task<IActionResult> DeleteFavoriteProduct(int id)
        {
            return Ok(IFavoriteProductService.Delete(id));
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpGet("CreateFavoriteProduct")]
        public async Task<IActionResult> CreateFavoriteProduct(CreateFavoriteProduct model)
        {
            return Ok(IFavoriteProductService.Create(GetCorrectUserId(), model));
        }
    }
}
