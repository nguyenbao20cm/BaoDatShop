using BaoDatShop.DTO.Role;
using BaoDatShop.DTO;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryAccountController : ControllerBase
    {
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;


        public HistoryAccountController(
           IHistoryAccountResponsitories IHistoryAccountResponsitories
          )
        {
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetHistoryAccount/{id}")]
        public async Task<IActionResult> GetHistoryAccount(string id)
        {
            var a = IHistoryAccountResponsitories.GetAll();
            if (a.Where(a => a.AccountID == id) == null)
                return Ok();
            return Ok(IHistoryAccountResponsitories.GetAll().Where(a => a.AccountID == id).ToList());
        }
    }
}
