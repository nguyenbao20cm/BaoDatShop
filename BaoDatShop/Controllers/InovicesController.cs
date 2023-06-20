﻿using BaoDatShop.DTO.Invoice;
using BaoDatShop.DTO.Product;
using BaoDatShop.DTO.Role;
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
    public class InovicesController : ControllerBase
    {
        private readonly IInvoiceService invoiceService;
        public InovicesController(IInvoiceService invoiceService)
        {
            this.invoiceService = invoiceService;
        }
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("CreateInvoice")]
        public async Task<IActionResult> CreateInvoice(CreateInvoiceRequest model)
        {
            return Ok(invoiceService.Create(GetCorrectUserId(), model));
        }
        
        [Authorize(Roles = UserRole.Costumer)]
        [HttpPost("GetAllInvoiceOfAccount")]
        public async Task<IActionResult> GetAllInvoiceOfAccount()
        {
            return Ok(invoiceService.GetAllInvoiceOfAccount(GetCorrectUserId()).OrderByDescending(a=>a.IssuedDate).ToList());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteInvoice/{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            if(invoiceService.DeleteInvoice(id)==true)
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
        [HttpGet("GetAllInoviceHUy")]
        public async Task<IActionResult> GetAllInoviceHUy()
        {
            return Ok(invoiceService.GetAll().Where(a=>a.OrderStatus==4).ToList());
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
        [HttpGet("GetAllInoviceTotalMonth/{year}")]
        public async Task<IActionResult> GetAllInoviceTotalMonth(string year)
        {
            return Ok(invoiceService.GetAllInoviceTotalMonth(year));
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
        public async Task<IActionResult> GetAllInoviceFilterByDate(string startDate,string endDate)
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
