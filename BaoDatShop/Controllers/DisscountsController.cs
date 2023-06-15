using BaoDatShop.DTO.AdvertisingPanel;
using BaoDatShop.DTO.Disscount;
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
    public class DisscountsController : ControllerBase
    {
        private readonly IDisscountService IDisscountService;
        public DisscountsController(IDisscountService IDisscountService)
        {
            this.IDisscountService = IDisscountService;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllDisscountStatusTrue")]
        public async Task<IActionResult> GetAllDisscountStatusTrue()
        {
            return Ok(IDisscountService.GetAllDisscountPanel());
        }
        [HttpGet("GetDisscountByProductId/{id}")]
        public async Task<IActionResult> GetDisscountByProductId(int id)
        {
            return Ok(IDisscountService.GetDisscountByProductId(id));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllDisscount")]
        public async Task<IActionResult> GetAllDisscount()
        {
            return Ok(IDisscountService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllDisscountStatusFalse")]
        public async Task<IActionResult> GetAllDisscountStatusFalse()
        {
            return Ok(IDisscountService.GetAllDisscountStatusFalse());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateDisscount")]
        public async Task<IActionResult> CreateDisscount(CreateDisscount model)
        {
            if (IDisscountService.Create(model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateDisscount/{id}")]
        public async Task<IActionResult> UpdateDisscount(int id, CreateDisscount model)
        {
            return Ok(IDisscountService.Update(id, model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteDisscount/{id}")]
        public async Task<IActionResult> DeleteDisscount(int id)
        {
            return Ok(IDisscountService.Delete(id));
        }
    }
}
