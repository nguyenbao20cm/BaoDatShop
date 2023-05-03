using BaoDatShop.DTO.NewDetail;
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
    public class NewDetailsController : ControllerBase
    {
        private readonly INewDetailService newDetailService;
        public NewDetailsController(INewDetailService newDetailService)
        {
            this.newDetailService = newDetailService;
        }
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
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateNewDetail")]
        public async Task<IActionResult> CreateNewDetail([FromForm] CreateNewDetailRequest model)
        {
            return Ok(newDetailService.Create(model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateNewDetail/{id}")]
        public async Task<IActionResult> UpdateNewDetail(int id,[FromForm] CreateNewDetailRequest model)
        {
            return Ok(newDetailService.Update(id,model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteNewDetail/{id}")]
        public async Task<IActionResult> DeleteNewDetail(int id)
        {
            return Ok(newDetailService.Delete(id));
        }
    }
}
