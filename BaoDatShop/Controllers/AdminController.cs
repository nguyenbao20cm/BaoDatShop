using BaoDatShop.DTO.AdvertisingPanel;
using BaoDatShop.DTO.NewDetail;
using BaoDatShop.DTO.News;
using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.ProductType;
using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRole.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IProductTypeService productypeService;
        private readonly IProductService productService;
        private readonly INewsService newsService;
        private readonly INewDetailService newDetailService ;
        private readonly IAdvertisingPanelService advertisingPanelService;
        private readonly IReviewService reviewService;
        public AdminController
        (
            IReviewService reviewService,
            IAdvertisingPanelService advertisingPanelService,
            INewDetailService newDetailService,
            INewsService newsService,
            IProductTypeService productypeService,
            IProductService productService
        )
        {
            this.reviewService = reviewService;
            this.advertisingPanelService=advertisingPanelService; 
            this.newDetailService=newDetailService;
            this.newsService = newsService;
            this.productypeService = productypeService;
            this.productService = productService;
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
        //Review
        [HttpGet("GetAllReview")]
        public async Task<IActionResult> GetAllReview()
        {
            return Ok(reviewService.GetAll());
        }
        [HttpPut("DeleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            return Ok(reviewService.Delete(id));
        }
        // ProductType
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
        [HttpPost("CreateProductType")]
        public async Task<IActionResult> CreateProductType([FromForm] CreateProductType model)
        {
            return Ok(productypeService.Create(model));
        }
        [HttpPut("UpdateProductType/{id}")]
        public async Task<IActionResult> UpdateProductType(int id, [FromForm] CreateProductType model)
        {
            return Ok(productypeService.Update(id, model));
        }
        [HttpPut("DeleteProductType/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            return Ok(productypeService.Delete(id));
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
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProduct model)
        {
            return Ok(productService.Create(model));
        }
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProduct model)
        {
            return Ok(productService.Update(id, model));
        }
        [HttpPut("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return Ok(productService.Delete(id));
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
        [HttpPost("CreateNew")]
        public async Task<IActionResult> CreateNew([FromForm] CreateNews model)
        {
            return Ok(newsService.Create(model));
        }
        [HttpPut("UpdateNew/{id}")]
        public async Task<IActionResult> UpdateNew(int id, [FromForm] CreateNews model)
        {
            return Ok(newsService.Update(id, model));
        }
        [HttpPut("DeleteNew/{id}")]
        public async Task<IActionResult> DeleteNew(int id)
        {
            return Ok(newsService.Delete(id));
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
        [HttpPost("CreateDetailNew")]
        public async Task<IActionResult> CreateNewDetail([FromForm] CreateNewDetail model)
        {
            return Ok(newDetailService.Create(model));
        }
        [HttpPut("UpdateNewDetail/{id}")]
        public async Task<IActionResult> UpdateNewDetail(int id, [FromForm] CreateNewDetail model)
        {
            return Ok(newDetailService.Update(id, model));
        }
        [HttpPut("DeleteNewDetail/{id}")]
        public async Task<IActionResult> DeleteNewDetail(int id)
        {
            return Ok(newDetailService.Delete(id));
        }
        //AdvertisingPanel
        [HttpGet("GetAllAdvertisingPanel")]
        public async Task<IActionResult> GetAllAdvertisingPanel()
        {
            return Ok(advertisingPanelService.GetAll());
        }
        
        [HttpPost("CreateAdvertisingPanel")]
        public async Task<IActionResult> CreateAdvertisingPanel([FromForm] CreateAdvertisingPanel model)
        {
            return Ok(advertisingPanelService.Create(model));
        }
        [HttpPut("UpdateAdvertisingPanel/{id}")]
        public async Task<IActionResult> UpdateAdvertisingPanel(int id, [FromForm] CreateAdvertisingPanel model)
        {
            return Ok(advertisingPanelService.Update(id, model));
        }
        [HttpPut("DeleteAdvertisingPanel/{id}")]
        public async Task<IActionResult> DeleteAdvertisingPanel(int id)
        {
            return Ok(advertisingPanelService.Delete(id));
        }
    }
}
