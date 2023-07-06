using BaoDatShop.DTO.Review;
using BaoDatShop.DTO.Role;
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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService reviewService;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        public ReviewsController(IReviewService reviewService, IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.reviewService = reviewService;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("CreateReview")]
        public async Task<IActionResult> CreateReview(ReviewRequest model)
        {
            return Ok(reviewService.Create(GetCorrectUserId(),model));
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPut("DeleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if(reviewService.Delete(id)==true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã ẩn bình luận có id: "+id ;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            }    
            return Ok(reviewService.Delete(id));
        }
    
        [HttpPost("CreateImageReview")]
        public async Task<IActionResult> CreateImageReview(IFormFile model)
        {
            return Ok(reviewService.CreateImageReview(model));
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPut("UpdateReview/{id}")]
        public async Task<IActionResult> UpdateReview(int IdReview, ReviewRequest model)
        {
            return Ok(reviewService.Update( GetCorrectUserId(), IdReview, model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteReviewByAdmin/{id}")]
        public async Task<IActionResult> DeleteReviewByAdmin(int id)
        {
          
            if (reviewService.Delete(id) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        [HttpGet("GetAllReviewProduct/{id}")]
        public async Task<IActionResult> GetAllReview(int id)
        {
            return Ok(reviewService.GetByIdProduct(id));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllReview")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(reviewService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllReviewStatusTrue")]
        public async Task<IActionResult> GetAllStatusTrue()
        {
            return Ok(reviewService.GetAllStatusTrue());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllReviewStatusFalse")]
        public async Task<IActionResult> GetAllStatusFalse()
        {
            return Ok(reviewService.GetAllStatusFalse());
        }

        [HttpGet("GetAverageStartReview/{id}")]
        public async Task<IActionResult> GetAverageStartReview(int id)
        {
            var a = reviewService.GetAll().Where(a => a.ProductId == id).ToList();
            float Average =0;
            float b = 0;
            foreach(var item in a)
            {
                b++;
                Average += item.Star;
            }
            float re = Average / b;
            return Ok(re);
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}
