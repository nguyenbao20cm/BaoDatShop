using BaoDatShop.DTO.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = UserRole.Costumer)]
    [ApiController]
    public class CustomerController : ControllerBase
    {
    }
}
