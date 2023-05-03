using BaoDatShop.DTO.News;
using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService newsService;
        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

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
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateNew")]
        public async Task<IActionResult> CreateNew(CreateNewsRequest model)
        {
            return Ok(newsService.Create(model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateNew/{id}")]
        public async Task<IActionResult> UpdateNew(int id, CreateNewsRequest model)
        {
            return Ok(newsService.Update(id, model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteNew/{id}")]
        public async Task<IActionResult> DeleteNew(int id)
        {
            return Ok(newsService.Delete(id));
        }
    }
}
