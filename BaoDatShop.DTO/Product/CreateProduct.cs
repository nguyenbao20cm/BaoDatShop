﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Product
{
    public class CreateProduct
    {
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        public string SKU { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        public string Name { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [DisplayName("Giá (VNĐ)")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        [DefaultValue(0)]
        public int Price { get; set; } = 0;

        [DisplayName("Tồn kho")]
        [DefaultValue(0)]
        public int Stock { get; set; } = 0;
        [DisplayName("Loại sản phẩm")]
        public int ProductTypeId { get; set; }
        
        // Navigation reference property cho khóa ngoại đến ProductType
        [DisplayName("Ảnh minh họa")]
        public string Image { get; set; }

     
      
    }
}
