﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BaoDatShop.DTO.Product
{
    public class CreateProductRequest
    {
    
       
        
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; } = 0;

        public int Stock { get; set; } = 0;
   
        public int ProductTypeId { get; set; }
        public string Image { get; set; }

     
      
    }
}
