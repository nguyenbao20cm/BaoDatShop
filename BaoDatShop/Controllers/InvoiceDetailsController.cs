﻿using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class InvoiceDetailsController : ControllerBase
    {
        private readonly IInvoiceDetailService invoiceDetailService;
        public InvoiceDetailsController(IInvoiceDetailService invoiceDetailService)
        {
            this.invoiceDetailService = invoiceDetailService;
        }
     
        [HttpGet("GetAllInvoiceDetails/{id}")]
        public async Task<IActionResult> GetAllNewDetail(int id)
        {
            return Ok(invoiceDetailService.GetAll(id));
        }
    }
}
