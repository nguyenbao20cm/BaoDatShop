﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Product
{
    public class GetAllProductResponse
    {
        public int Id { get; set; }


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

        public int ProductTypeId { get; set; }

        [DisplayName("Ảnh minh họa")]
        public string Image { get; set; }

    

     
    
    }
}
