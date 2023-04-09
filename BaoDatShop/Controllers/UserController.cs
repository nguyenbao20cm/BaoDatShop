using BaoDatShop.DTO.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
         [HttpGet("signup")]
        public async Task<IActionResult> signup()
        {
            return Ok();
        }
    }
}
