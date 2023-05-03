using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BaoDatShop.DTO.Cart
{
    public class CreateCartRequest
    {
       
        public string AccountId { get; set; }
        
        public int ProductId { get; set; }
   
        public int Quantity { get; set; } = 1;
    }
}
