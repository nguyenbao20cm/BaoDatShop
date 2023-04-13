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

    public class UserController : ControllerBase
    {
        private readonly IAdvertisingPanelService advertisingPanelService;
        private readonly IProductTypeService productypeService;
        private readonly IProductService productService;
        private readonly INewsService newsService;
        private readonly INewDetailService newDetailService;
        private readonly IReviewService reviewService;
        public UserController
            (
            IReviewService reviewService,
            IAdvertisingPanelService advertisingPanelService,
            INewsService newsService,
            INewDetailService newDetailService,
            IProductTypeService productypeService, 
            IProductService productService
            )
        {
            this.reviewService=reviewService; 
            this.advertisingPanelService=advertisingPanelService; 
            this.newsService = newsService;
            this.newDetailService=newDetailService;
            this.productypeService = productypeService;
            this.productService = productService;
        }
        //Review
        [HttpGet("GetAllReviewProduct/{id}")]
        public async Task<IActionResult> GetAllReview(int id)
        {
            return Ok(reviewService.GetByIdProduct(id));
        }
        //ProductType
        [HttpGet("GetAllProductType")]
        public async Task<IActionResult> GetAllProductType()
        {
            return Ok(productypeService.GetAll());
        }
        [HttpGet("GetProductTypeById/{id}")]
        public async Task<IActionResult> GetProductTypeById(int id)
        {
            return Ok(productypeService.GetById(id));
        }
        //Product
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(productService.GetAll());
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return Ok(productService.GetById(id));
        }
        //New
        [HttpGet("GetAllNew")]
        public async Task<IActionResult> GetAllNew()
        {
            return Ok(newsService.GetAll());
        }
        [HttpGet("GetNewById/{id}")]
        public async Task<IActionResult> GetNewById(int id)
        {
            return Ok(newsService.GetById(id));
        }
        //NewDetail
        [HttpGet("GetAllNewDetail")]
        public async Task<IActionResult> GetAllNewDetail()
        {
            return Ok(newDetailService.GetAll());
        }
        [HttpGet("GetNewDetailById/{id}")]
        public async Task<IActionResult> GetNewByDetailId(int id)
        {
            return Ok(newDetailService.GetById(id));
        }
        // AdvertisingPanel
        [HttpGet("GetAllAdvertisingPanel")]
        public async Task<IActionResult> GetAllAdvertisingPanel()
        {
            return Ok(advertisingPanelService.GetAll());
        }
    }
}
