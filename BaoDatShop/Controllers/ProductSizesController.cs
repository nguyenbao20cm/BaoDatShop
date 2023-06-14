﻿using BaoDatShop.DTO.ProductSize;
using BaoDatShop.DTO.ProductType;
using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSizesController : ControllerBase
    {
        private readonly IProductSizeService productSizeService;
        public ProductSizesController(IProductSizeService productSizeService)
        {
            this.productSizeService = productSizeService;
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllProductSizeStatusTrue")]//status true
        public async Task<IActionResult> GetProductType()
        {
            return Ok(productSizeService.GetAllProductTypeStatusTrue());
        }
        [HttpGet("GetAllProductSize")]// all status
        public async Task<IActionResult> GetAllProductType()
        {
            return Ok(productSizeService.GetAll());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllProductSizeStatusFalse")]//  status false
        public async Task<IActionResult> GetAllProductTypeStatusFalse()
        {
            return Ok(productSizeService.GetAllProductTypeStatusFalse());
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("GetAllImportPrice/{year}")]//  status false
        public async Task<IActionResult> GetAllImportPrice(string year)
        {
            return Ok(productSizeService.GetAllImportPrice(year));
        }
        [HttpGet("GetProductSizeById/{id}")]
        public async Task<IActionResult> GetProductTypeById(int id)
        {
            return Ok(productSizeService.GetById(id));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPost("CreateProductSize")]
        public async Task<IActionResult> CreateProductSize(CreateProductSize model)
        {
            if (model.Name == string.Empty) return Ok("Không được để trống");
            if (productSizeService.Create(model) == true) return Ok("Thành công");
            else return Ok("Thất bại");

            return Ok(productSizeService.Create(model));
        }
        [Authorize(Roles = UserRole.Admin)]
        [HttpPut("UpdateProductSize/{id}")]
        public async Task<IActionResult> UpdateProductType(int id, UpdateProductSize model)
        {

            if (productSizeService.Update(id, model) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
        // [Authorize(Roles = UserRole.Admin)]
        [HttpPut("DeleteProductSize/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            if (productSizeService.Delete(id) == true)
                return Ok("Thành công");
            else
                return Ok("Thất bại");
        }
    }
}