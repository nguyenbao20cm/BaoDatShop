using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.CreateFavoriteProduct;
using BaoDatShop.DTO.Role;
using BaoDatShop.DTO.Voucher;
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
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherService IVoucherService;
        private readonly IEmailSender IEmailSender;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        public VouchersController(IHistoryAccountResponsitories IHistoryAccountResponsitories,IVoucherService IVoucherService, IEmailSender IEmailSender)
        {
            this.IVoucherService = IVoucherService;
            this.IEmailSender = IEmailSender;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
        }
        [HttpPost("SendVoucher")]
        public async Task<IActionResult> Index(SendVoucher model)
        {
            IEmailSender.SendEmailAsync(model);
            return Ok("ok");
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllVoucher")]
        public async Task<IActionResult> GetAllVoucher()
        {
            return Ok(IVoucherService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GetVoucherByid/{id}")]
        public async Task<IActionResult> GetVoucherByid(int id)
        {
            return Ok(IVoucherService.GetAll().Where(a=>a.Id==id).FirstOrDefault());
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("ValidationVoucher")]
        public async Task<IActionResult> ValidationVoucher(ValidationVoucher ValidationVoucher)
        {
            if (IVoucherService.ValidationVoucher(ValidationVoucher.Total, ValidationVoucher.Name) == null) return Ok("null");
            return Ok(IVoucherService.ValidationVoucher(ValidationVoucher.Total,ValidationVoucher.Name));
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpPut("DeleteVoucher/{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            if(IVoucherService.Delete(id)==true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã khóa đi Voucher có id=" + id;
                IHistoryAccountResponsitories.Create(ab);
                return Ok(true);
            }   
            else
                return Ok(false);
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpPut("ActiveVoucher/{id}")]
        public async Task<IActionResult> ActiveVoucher(int id)
        {
            if (IVoucherService.ActiveVoucher(id) == true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã kích hoạt lại Voucher có id=" + id;
                IHistoryAccountResponsitories.Create(ab);
                return Ok(true);
            }
            else
                return Ok(false);
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpPost("CreateVoucher")]
        public async Task<IActionResult> CreateVoucher(CreateVoucher model)
        {
            var tam = IVoucherService.GetAll();
            foreach(var item in tam)
            {
                if (model.Name == item.Name)
                    return Ok(1);
            }    
            if (IVoucherService.Create(model) == true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã tạo thành công Voucher "+model.Title;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            }   
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpPut("UpdateVoucher/{id}")]
        public async Task<IActionResult> UpdateVoucher(int id,CreateVoucher model)
        {
            
            if (IVoucherService.Update(id,model) == true)
            {
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã chỉnh sửa Voucher " + model.Title;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            }
            else
                return Ok(false);
        }
        [HttpPut("GetALLVOucherOnCustomer")]
        public async Task<IActionResult> GetALLVOucherOnCustomer()
        {

                return Ok(IVoucherService.GetAll().Where(a=>a.Status==true));
        }
    }
}
