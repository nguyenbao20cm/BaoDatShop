using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRole.Admin)]
    public class AdminController : ControllerBase
    {
        [HttpGet("signup")]
        public async Task<IActionResult> signup()
        {
            return Ok();
        }
    }
}
