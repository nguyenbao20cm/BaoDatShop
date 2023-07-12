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
using System.Security.Claims;

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
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer + "," + UserRole.Staff + "," + UserRole.StaffKHO)]
        [HttpGet("GetHistoryAccount/{id}")]
        public async Task<IActionResult> GetHistoryAccount(string id)
        {
            var a = IHistoryAccountResponsitories.GetAll();
            if (a.Where(a => a.AccountID == id) == null)
                return Ok();
            return Ok(IHistoryAccountResponsitories.GetAll().Where(a => a.AccountID == id).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer + "," + UserRole.Staff + "," + UserRole.StaffKHO)]
        [HttpGet("GetHistoryAccountFilter/{startday},{endday}")]
        public async Task<IActionResult> GetHistoryAccountFilter(string startday,string endday)
        {
            return Ok(IHistoryAccountResponsitories.GetAll().Where(a => a.AccountID == GetCorrectUserId()).Where(a => a.Datetime.Date >= DateTime.Parse(startday)).Where(a => a.Datetime.Date <= DateTime.Parse(endday)).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer + "," + UserRole.Staff + "," + UserRole.StaffKHO)]
        [HttpGet("GetHistoryAllAccount")]
        public async Task<IActionResult> GetHistoryAccountAdmin()
        {
            var a = IHistoryAccountResponsitories.GetAll();
            if (a.Where(a => a.AccountID == GetCorrectUserId()) == null)
                return Ok();
            return Ok(IHistoryAccountResponsitories.GetAll().Where(a => a.AccountID == GetCorrectUserId()).OrderByDescending(a=>a.Datetime).ToList());
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
    }
}
