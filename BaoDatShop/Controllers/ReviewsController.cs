using BaoDatShop.DTO.Review;
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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService reviewService;
        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("CreateReview")]
        public async Task<IActionResult> CreateReview(ReviewRequest model,string AccountId)
        {
            return Ok(reviewService.Create(GetCorrectUserId(),model));
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPut("DeleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            return Ok(reviewService.Delete(id));
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
            return Ok(reviewService.Delete(id));
        }
        [HttpGet("GetAllReviewProduct/{id}")]
        public async Task<IActionResult> GetAllReview(int id)
        {
            return Ok(reviewService.GetByIdProduct(id));
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}
