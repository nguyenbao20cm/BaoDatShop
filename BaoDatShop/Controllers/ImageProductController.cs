using BaoDatShop.DTO.CreateImageProduct;
using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
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
    public class ImageProductController : ControllerBase
    {
        private readonly IImageProductService IImageProductService;
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration configuration;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        public ImageProductController(IConfiguration configuration,AppDbContext context,
            IWebHostEnvironment _environment,
            IImageProductService IImageProductService, IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.context = context;this._environment = _environment;
            this.configuration = configuration;
            this.IImageProductService = IImageProductService;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
        }

        [Authorize(Roles = UserRole.Admin)]
        [HttpDelete("DeleteImageProduct/{id}")]
        public async Task<IActionResult> UpdateImageProduct(int id)
        {
            context.Remove(context.ImageProduct.Where(a => a.Id == id).FirstOrDefault());
           var check= context.SaveChanges();
            if (check < 0) return Ok(false);
            else
                return Ok(true);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateImageProduct/{id}")]
        public async Task<IActionResult> UpdateImageProduct(int id, CreateImageProduct model)
        {
            if (IImageProductService.Update(id, model) == true)
            {
                  
                return Ok("Thành công");
            }    
               
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateImageProductWithImage/{Id},{ProductId}")]
        public async Task<IActionResult> UpdateImageProductWithImage(int id,int ProductId, IFormFile model)
        {
            ImageProduct mo = context.ImageProduct.Where(a=>a.Id==id).FirstOrDefault();
            mo.ProductId = ProductId;
            context.Update(mo);
            var check = context.SaveChanges();
            if (check > 0)
            {
                var fileName = mo.Id + ".jpg";
                var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "ImageProduct");
                var uploadPath = Path.Combine(uploadFolder, fileName);
                using (FileStream fs = System.IO.File.Create(uploadPath))
                {
                    model.CopyTo(fs);
                    fs.Flush();
                }
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã chỉnh sửa ảnh phụ của sản phẩm" + context.Product.Where(a => a.Id == ProductId).FirstOrDefault().Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok(true);
            }
            return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateImageProduct/{Id},{ProductId}")]
        public async Task<IActionResult> UpdateImageProduct(int id, int ProductId)
        {
            ImageProduct mo = context.ImageProduct.Where(a => a.Id == id).FirstOrDefault();
            mo.ProductId = ProductId;
            context.Update(mo);
            var check = context.SaveChanges();
            if (check > 0)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã chỉnh sửa ảnh phụ của sản phẩm " + context.Product.Where(a => a.Id == ProductId).FirstOrDefault().Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok(true);
            }
            return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateImagesProduct/{ProductId}")]
        public async Task<IActionResult> CreateImagesProduct(int ProductId, IFormFile model)
        {
            ImageProduct mo = new();
            mo.ProductId = ProductId;
            mo.Status = true;
            mo.Image = "";
            context.Add(mo);
            var check = context.SaveChanges();
            if (check>0)
            {
                var fileName = mo.Id + ".jpg";
                var a = context.ImageProduct.Where(a => a.Id == mo.Id).FirstOrDefault();
                a.Image = fileName;
                context.Update(a);
                context.SaveChanges();
                var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "ImageProduct");
                var uploadPath = Path.Combine(uploadFolder, fileName);
                    using (FileStream fs = System.IO.File.Create(uploadPath))
                    {
                        model.CopyTo(fs);
                        fs.Flush();
                    }
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã tạo ảnh phụ của sản phẩm "+ context.Product.Where(a=>a.Id== ProductId).FirstOrDefault().Name;
                IHistoryAccountResponsitories.Create(ab);
                return Ok(true);
            }
            return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllImageProduct")]
        public async Task<IActionResult> GetAllImageProduct()
        {
            return Ok(IImageProductService.GetAll());
        }
        [HttpGet("GetAllImageProductStatusTrue")]
        public async Task<IActionResult> GetAllImageProductStatusTrue()
        {
            return Ok(IImageProductService.GetAllImageProductStatusTrue());
        }

        [HttpGet("GetAllImageProductStatusFalse")]
        public async Task<IActionResult> GetAllImageProductStatusFalse()
        {
            return Ok(IImageProductService.GetAllImageProductStatusFalse());
        }
        [HttpGet("GetImageProductById/{id}")]
        public async Task<IActionResult> GetImageProductById(int id)
        {
            return Ok(IImageProductService.GetById(id));

        }
        [HttpGet("GetAllImageProductById/{id}")]
        public async Task<IActionResult> GetAllImageProductById(int id)
        {
            return Ok(IImageProductService.GetAllImageProductById(id));

        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }

    }
}
