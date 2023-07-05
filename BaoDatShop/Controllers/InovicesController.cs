using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InovicesController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IInvoiceService invoiceService;
        private readonly IInvoiceResponsitories IInvoiceResponsitories;
        private readonly IProductSizeResponsitories IProductSizeResponsitories;
        private readonly IProductSizeService IProductSizeService;
        public InovicesController(IInvoiceService invoiceService, IProductSizeService IProductSizeService,
            AppDbContext context, IInvoiceResponsitories IInvoiceResponsitories, IProductSizeResponsitories IProductSizeResponsitories)
        {
            this.IProductSizeService = IProductSizeService;
            this.context = context;
            this.invoiceService = invoiceService;
            this.IInvoiceResponsitories = IInvoiceResponsitories;
            this.IProductSizeResponsitories = IProductSizeResponsitories;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllChiPhi")]
        public async Task<IActionResult> GetAllChiPhi( )
        {
            
            var a = DateTime.Now;
            var ngayHienTai = new DateTime(a.Year,a.Month,a.Day);
            var ngayDauTien= new DateTime(2023, 7, 1);
            List<Chiphi> ab = new();
            TimeSpan Time = ngayHienTai - ngayDauTien;
            for(var i= 0; i<= Time.Days; i++)
            {
                Chiphi tam = new();
                tam.DateTime = ngayDauTien.AddDays(i);
                var tong1 = 0;
                var ab1=context.Invoice
                    .Where(a => a.PaymentMethods == true).Where(a => a.IssuedDate.Date == tam.DateTime.Date && a.IssuedDate.Year == tam.DateTime.Year && a.IssuedDate.Month == tam.DateTime.Month).ToList();
                foreach(var ch in ab1)
                {
                    tong1++;
                }    
                tam.ChiPhiVanChuyen= tong1*20000;
                var tong2 = 0;
                var ab12 = context.ProductSize.
                    Where(a => a.IssuedDate.Date == tam.DateTime.Date && a.IssuedDate.Year == tam.DateTime.Year && a.IssuedDate.Month == tam.DateTime.Month).ToList();
                foreach(var ch1 in ab12)
                {
                    tong2 += ch1.ImportPrice;
                }
                tam.ChiPhiNhap = tong2;
                var tong3 = 0;
                var ab123 = context.Invoice.
                    Where(a => a.IssuedDate.Date == tam.DateTime.Date && a.IssuedDate.Year == tam.DateTime.Year && a.IssuedDate.Month == tam.DateTime.Month).ToList();
                foreach (var ch3 in ab123)
                {
                    tong3 += ch3.Total;
                }
                tam.ThuNhap = tong3;
                ab.Add(tam);
            }
            return Ok(ab);
        }

        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("CreateInvoice")]
        public async Task<IActionResult> CreateInvoice(CreateInvoiceRequest model)
        {
            return Ok(invoiceService.Create(GetCorrectUserId(), model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("GetMonthInvoice")]
        public async Task<IActionResult> GetMonthInvoice(YearAndMonth model)
        {

           
            var ImportPrice = 0;
            var ImportPiceList = IProductSizeResponsitories.GetAll().Where(a => a.IssuedDate.Month == model.Month).Where(a => a.IssuedDate.Year == model.Year).ToList();
            foreach (var aba in ImportPiceList)
            {
                ImportPrice += aba.ImportPrice * aba.Stock;
            }
            var Total = 0;
            var TotalList = IInvoiceResponsitories.GetAll().Where(a=>a.OrderStatus==5).Where(a => a.IssuedDate.Month == model.Month).Where(a => a.IssuedDate.Year == model.Year).ToList();
            foreach (var abc in TotalList)
            {
                Total += abc.Total;
            }
            var b = Total - ImportPrice;
            var c = Total - ImportPrice;
            return Ok(c);
        }

        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("CreateInvoiceNow")]
        public async Task<IActionResult> CreateInCreateInvoiceNowvoice(CreateInvoiceNow model)
        {
            return Ok(invoiceService.CreateInvoiceNow(GetCorrectUserId(), model));
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("GetAllInvoiceOfAccount")]
        public async Task<IActionResult> GetAllInvoiceOfAccount()
        {
            return Ok(invoiceService.GetAllInvoiceOfAccount(GetCorrectUserId()).OrderByDescending(a => a.IssuedDate).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteInvoice/{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            if (invoiceService.DeleteInvoice(id) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPut("DeleteInvoiceByCostumer/{id}")]
        public async Task<IActionResult> DeleteInvoiceByCostumer(int id)
        {
            if (invoiceService.DeleteInvoiceByCostumer(id) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInovice")]
        public async Task<IActionResult> GetAllInovice()
        {
            return Ok(invoiceService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetQuanlityAllInovice")]
        public async Task<IActionResult> GetQuanlityAllInovice()
        {
            return Ok(invoiceService.GetAll().Where(a=>a.OrderStatus!=4).ToList().Count);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInoviceHUy")]
        public async Task<IActionResult> GetAllInoviceHUy()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 4).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInoviceHT")]
        public async Task<IActionResult> GetAllInoviceHT()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 5).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInoviceCXN")]
        public async Task<IActionResult> GetAllInoviceCXN()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 1).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInoviceCB")]
        public async Task<IActionResult> GetAllInoviceCB()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 2).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInoviceDaG")]
        public async Task<IActionResult> GetAllInoviceDG()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 6).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInoviceDangG")]
        public async Task<IActionResult> GetAllInoviceDangG()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 3).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer)]
        [HttpGet("GetInvoiceById/{id}")]
        public async Task<IActionResult> GetAllInovice(int id)
        {
            return Ok(invoiceService.GetById(id));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("ProfitForyear/{year}")]
        public async Task<IActionResult> ProfitForyear(int year)
        {
            return Ok(invoiceService.ProfitForyear(year));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("ProfitForYearAgo/{year}")]
        public async Task<IActionResult> ProfitForYearAgo(int year)
        {
            return Ok(invoiceService.ProfitForyear(year));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInoviceTotalMonth/{year}")]
        public async Task<IActionResult> GetAllInoviceTotalMonth(string year)
        {
            return Ok(invoiceService.GetAllInoviceTotalMonth(year));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllInoviceTotal/{year}")]
        public async Task<IActionResult> GetAllInoviceTotal(int year)
        {
            var a = invoiceService.GetAll().Where(a => a.OrderStatus == 5).Where(a => a.IssuedDate.Year == year).ToList();
            var tong = 0;
            foreach(var item in a)
            {
                tong += item.Total;
            }    
            return Ok(tong);
        }
        private int PhivanChuyen(int date)
        {
            var a = invoiceService.GetAll().Where(a => a.IssuedDate.Month == date).Where(a=>a.OrderStatus!=1|| a.OrderStatus != 2).ToList();
            int tong = 0;
            foreach(var item in a)
            {
                    tong += 20000;
            }
            return tong;
        }

        private int PhivanChuyenTrongNam(int year,int date)
        {
            var a = invoiceService.GetAll().Where(a => a.OrderStatus != 1 || a.OrderStatus != 2).Where(a => a.IssuedDate.Month == date&&  a.IssuedDate.Year == year).ToList();
            int tong = 0;
            foreach (var item in a)
            {
                    tong += 20000;
            }
            return tong;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetPhiVanChuyen/{year}")]
        public async Task<IActionResult> GetProfit(int year)
        {
            Month a = new();
             a.Month1 = PhivanChuyenTrongNam(year,1);
             a.Month2 = PhivanChuyenTrongNam(year,2);
             a.Month3 = PhivanChuyenTrongNam(year,3);
             a.Month4 = PhivanChuyenTrongNam(year,4);
             a.Month5 = PhivanChuyenTrongNam(year,5);
             a.Month6 = PhivanChuyenTrongNam(year,6);
             a.Month7 = PhivanChuyenTrongNam(year,7);
             a.Month8 = PhivanChuyenTrongNam(year,8);
             a.Month9 = PhivanChuyenTrongNam(year,9);
             a.Month10 =PhivanChuyenTrongNam(year,10);
             a.Month11 = PhivanChuyenTrongNam(year,11);
             a.Month12 = PhivanChuyenTrongNam(year,12);
            return Ok(a);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetProfit/{year}")]
        public async Task<IActionResult> GetProfit(string year)
        {
          var b=  invoiceService.GetAllInoviceTotalMonth(year);
            var c = IProductSizeService.GetAllImportPrice(year);
           
            Month a = new();
            a.Month1 = b.Month1 - c.Month1- PhivanChuyenTrongNam(int.Parse(year),1);
            a.Month2 = b.Month2 - c.Month2- PhivanChuyenTrongNam(int.Parse(year),2);
            a.Month3 = b.Month3 - c.Month3- PhivanChuyenTrongNam(int.Parse(year),3);
            a.Month4 = b.Month4 - c.Month4- PhivanChuyenTrongNam(int.Parse(year),4);
            a.Month5 = b.Month5 - c.Month5- PhivanChuyenTrongNam(int.Parse(year),5);
            a.Month6 = b.Month6 - c.Month6- PhivanChuyenTrongNam(int.Parse(year),6);
            a.Month7 = b.Month7 - c.Month7- PhivanChuyenTrongNam(int.Parse(year),7);
            a.Month8 = b.Month8 - c.Month8- PhivanChuyenTrongNam(int.Parse(year), 8);
            a.Month9 = b.Month9 - c.Month9 - PhivanChuyenTrongNam(int.Parse(year), 9);
            a.Month10 = b.Month10 - c.Month10- PhivanChuyenTrongNam(int.Parse(year), 10);
            a.Month11 = b.Month11 - c.Month11- PhivanChuyenTrongNam(int.Parse(year), 11);
            a.Month12 = b.Month12 - c.Month12 - PhivanChuyenTrongNam(int.Parse(year), 12);
            return Ok(a);
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateInovice/{id}")]
        public async Task<IActionResult> UpdateInovice(int id, UpdateInvoice model)
        {
            if (model.pay == null) return Ok("Thất bại");
            if (model.orderStatus == null) return Ok("Thất bại");
            if (model.shippingadress == null) return Ok("Thất bại");
            if (model.shippingphone == null) return Ok("Thất bại");
            if (invoiceService.UpdateInovice(id, model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin)]

        [HttpGet("GetAllInoviceFilterByDate/{startDate},{endDate}")]
        public async Task<IActionResult> GetAllInoviceFilterByDate(string startDate, string endDate)
        {
            return Ok(invoiceService.GetAllInoviceFilterByDate(startDate, endDate));
        }
        private string GetCorrectUserId()
        {
            var a = (ClaimsIdentity)User.Identity;
            var result = a.FindFirst("UserId").Value;
            return result;
        }

    }
}
