using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using Eshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InovicesController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IInvoiceDetailService IInvoiceDetailService;
        private readonly IWebHostEnvironment IWebHostEnvironment;
        private readonly IProductService IProductService;
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        private readonly IInvoiceService invoiceService;
        private readonly IInvoiceResponsitories IInvoiceResponsitories;
        private readonly IProductSizeResponsitories IProductSizeResponsitories;
        private readonly IProductSizeService IProductSizeService;
        private readonly IImportInvoiceResponsitories IImportInvoiceResponsitories;
        public InovicesController(IInvoiceService invoiceService, IProductSizeService IProductSizeService,
            IImportInvoiceResponsitories IImportInvoiceResponsitories,
            IProductService IProductService, IInvoiceDetailService IInvoiceDetailService,
            IWebHostEnvironment IWebHostEnvironment,
            AppDbContext context, IInvoiceResponsitories IInvoiceResponsitories, IProductSizeResponsitories IProductSizeResponsitories, IHistoryAccountResponsitories IHistoryAccountResponsitories)
        {
            this.IProductService = IProductService;
            this.IInvoiceDetailService = IInvoiceDetailService;
            this.IImportInvoiceResponsitories = IImportInvoiceResponsitories;
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
            this.IProductSizeService = IProductSizeService;
            this.context = context;
            this.IWebHostEnvironment = IWebHostEnvironment;
            this.invoiceService = invoiceService;
            this.IInvoiceResponsitories = IInvoiceResponsitories;
            this.IProductSizeResponsitories = IProductSizeResponsitories;
        }
     
        //[Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GeneratePDF")]
        public async Task<IActionResult> GeneratePDF(int InvoiceNo)
        {
            var document = new PdfDocument();
            string[] copies = { "Customer copy", "Comapny Copy" };
            for (int i = 0; i < copies.Length; i++)
            {
                var header = invoiceService.GetAll().Where(a=>a.Id==InvoiceNo).FirstOrDefault();
                List<InvoiceDetail> detail = IInvoiceDetailService.GetAll().Where(a=>a.InvoiceId==InvoiceNo).ToList();
                string htmlcontent = "<div style='width:100%; text-align:center'>";
                htmlcontent += "<img style='width:80px;height:80%' src=''https://localhost:7067/wwwroot/Image/Footer/'" + "1.jpg" + "'   />";
                htmlcontent += "<h2>" + copies[i] + "</h2>";
                htmlcontent += "<h2>Welcome to Nihira Techiees</h2>";

                if (header != null)
                {
                    htmlcontent += "<h2> Invoice No:" + header.Id + " & Invoice Date:" + header.IssuedDate + "</h2>";
                    htmlcontent += "<h3> Customer : " + header.NameCustomer + "</h3>";
                    htmlcontent += "<p>" + header.ShippingAddress + "</p>";
                    htmlcontent += "<h3> Contact : 9898989898 & Email :ts@in.com </h3>";
                    htmlcontent += "<div>";
                }
                htmlcontent += "<table style ='width:100%; border: 1px solid #000'>";
                htmlcontent += "<thead style='font-weight:bold'>";
                htmlcontent += "<tr>";
                htmlcontent += "<td style='border:1px solid #000'> Product Code </td>";
                htmlcontent += "<td style='border:1px solid #000'> Description </td>";
                htmlcontent += "<td style='border:1px solid #000'>Qty</td>";
                htmlcontent += "<td style='border:1px solid #000'>Price</td >";
                htmlcontent += "<td style='border:1px solid #000'>Total</td>";
                htmlcontent += "</tr>";
                htmlcontent += "</thead >";

                htmlcontent += "<tbody>";
                if (detail != null && detail.Count > 0)
                {
                    detail.ForEach(item =>
                    {
                        htmlcontent += "<tr>";
                        htmlcontent += "<td>" + item.ProductSize.Product.SKU + "</td>";
                        htmlcontent += "<td>" + item.ProductSize.Product.Name + "</td>";
                        htmlcontent += "<td>" + item.Quantity + "</td >";
                        htmlcontent += "<td>" + item.UnitPrice + "</td>";
                        htmlcontent += "<td> " + item.UnitPrice*item.Quantity + "</td >";
                        htmlcontent += "</tr>";
                    });
                }
                htmlcontent += "</tbody>";

                htmlcontent += "</table>";
                htmlcontent += "</div>";

                htmlcontent += "<div style='text-align:right'>";
                htmlcontent += "<h1> Summary Info </h1>";
                htmlcontent += "<table style='border:1px solid #000;float:right' >";
                htmlcontent += "<tr>";
                htmlcontent += "<td style='border:1px solid #000'> Summary Total </td>";
                htmlcontent += "<td style='border:1px solid #000'> Summary Tax </td>";
                htmlcontent += "<td style='border:1px solid #000'> Summary NetTotal </td>";
                htmlcontent += "</tr>";
                if (header != null)
                {
                    htmlcontent += "<tr>";
                    htmlcontent += "<td style='border: 1px solid #000'> " + header.Total + " </td>";
                    //htmlcontent += "<td style='border: 1px solid #000'>" + header.Tax + "</td>";
                    //htmlcontent += "<td style='border: 1px solid #000'> " + header.NetTotal + "</td>";
                    htmlcontent += "</tr>";
                }
                htmlcontent += "</table>";
                htmlcontent += "</div>";

                htmlcontent += "</div>";

                PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);
            }
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Invoice_" + InvoiceNo + ".pdf";
            return File(response, "application/pdf", Filename);
        }

        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
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
                var ab12 = context.ImportInvoice.
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
            
                if (tam.ThuNhap!=0||tam.ChiPhiNhap!=0||tam.ChiPhiVanChuyen!=0)
                ab.Add(tam);
            }
            return Ok(ab);
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GetAllChiPhiFliter/{startday},{endday}")]
        public async Task<IActionResult> GetAllChiPhiFliter(string startday,string endday)
        {
            var ngayHienTai =  DateTime.Parse(startday);
            var ngayDauTien = DateTime.Parse(endday);
            List<Chiphi> ab = new();
            TimeSpan Time = ngayHienTai - ngayDauTien;
            for (var i = 0; i <= Time.Days; i++)
            {
                Chiphi tam = new();
                tam.DateTime = ngayDauTien.AddDays(i);
                var tong1 = 0;
                var ab1 = context.Invoice
                    .Where(a => a.PaymentMethods == true).Where(a => a.IssuedDate.Date == tam.DateTime.Date && a.IssuedDate.Year == tam.DateTime.Year && a.IssuedDate.Month == tam.DateTime.Month).ToList();
                foreach (var ch in ab1)
                {
                    tong1++;
                }
                tam.ChiPhiVanChuyen = tong1 * 20000;
                var tong2 = 0;
                var ab12 = context.ImportInvoice.
                    Where(a => a.IssuedDate.Date == tam.DateTime.Date && a.IssuedDate.Year == tam.DateTime.Year && a.IssuedDate.Month == tam.DateTime.Month).ToList();
                foreach (var ch1 in ab12)
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
                if (tam.ThuNhap != 0 || tam.ChiPhiNhap != 0 || tam.ChiPhiVanChuyen != 0)
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
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpPost("GetMonthInvoice")]
        public async Task<IActionResult> GetMonthInvoice(YearAndMonth model)
        {

           
            var ImportPrice = 0;
            var ImportPiceList = IImportInvoiceResponsitories.GetAll().Where(a => a.IssuedDate.Month == model.Month).Where(a => a.IssuedDate.Year == model.Year).ToList();
            foreach (var aba in ImportPiceList)
            {
                ImportPrice += aba.ImportPrice * aba.Quantity;
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
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
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
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GetAllInovice")]
        public async Task<IActionResult> GetAllInovice()
        {
            return Ok(invoiceService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff+","+UserRole.StaffKHO)]
        [HttpGet("GetQuanlityAllInovice")]
        public async Task<IActionResult> GetQuanlityAllInovice()
        {
            return Ok(invoiceService.GetAll().Where(a=>a.OrderStatus!=4).ToList().Count);
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GetAllInoviceHUy")]
        public async Task<IActionResult> GetAllInoviceHUy()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 4).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GetAllInoviceHT")]
        public async Task<IActionResult> GetAllInoviceHT()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 5).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GetAllInoviceCXN")]
        public async Task<IActionResult> GetAllInoviceCXN()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 1).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GetAllInoviceCB")]
        public async Task<IActionResult> GetAllInoviceCB()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 2).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GetAllInoviceDaG")]
        public async Task<IActionResult> GetAllInoviceDG()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 6).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GetAllInoviceDangG")]
        public async Task<IActionResult> GetAllInoviceDangG()
        {
            return Ok(invoiceService.GetAll().Where(a => a.OrderStatus == 3).ToList());
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer + "," + UserRole.Staff)]
        [HttpGet("GetInvoiceById/{id}")]
        public async Task<IActionResult> GetAllInovice(int id)
        {
            return Ok(invoiceService.GetById(id));
        }
        [Authorize(Roles = UserRole.Admin  + "," + UserRole.Staff + "," + UserRole.StaffKHO)]
        [HttpGet("ProfitForyear/{year}")]
        public async Task<IActionResult> ProfitForyear(int year)
        {
            return Ok(invoiceService.ProfitForyear(year));
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Costumer + "," + UserRole.Staff)]
        [HttpGet("ProfitForYearAgo/{year}")]
        public async Task<IActionResult> ProfitForYearAgo(int year)
        {
            return Ok(invoiceService.ProfitForyear(year));
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpGet("GetAllInoviceTotalMonth/{year}")]
        public async Task<IActionResult> GetAllInoviceTotalMonth(string year)
        {
            return Ok(invoiceService.GetAllInoviceTotalMonth(year));
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.StaffKHO + "," + UserRole.Staff)]
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
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
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
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
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
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]
        [HttpPut("UpdateInovice/{id}")]
        public async Task<IActionResult> UpdateInovice(int id, UpdateInvoice model)
        {
            //if (model.pay == null) return Ok("Thất bại");
            if (model.orderStatus == null) return Ok("Thất bại");
            if (model.shippingadress == null) return Ok("Thất bại");
            if (model.shippingphone == null) return Ok("Thất bại");
            if (invoiceService.UpdateInovice(id, model) == true)
            {
                if (model.orderStatus == 5)
                {
                    foreach (var item in context.InvoiceDetail.Include(a=>a.ProductSize).Include(a => a.ProductSize.Product).Where(a => a.InvoiceId == id))
                    {
                        var b = IProductService.GetById(item.ProductSize.ProductId);
                        b.CountSell = b.CountSell + item.Quantity;
                        context.Update(b);
                    }
                }
                context.SaveChanges();
                HistoryAccount ab = new();
                ab.AccountID = GetCorrectUserId(); ab.Datetime = DateTime.Now;
                ab.Content = "Đã cập nhật hóa đơn có id "+ id;
                IHistoryAccountResponsitories.Create(ab);
                return Ok("Thành công");
            }    
            else
                return Ok("Thất bại");
        }
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Staff)]  
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
