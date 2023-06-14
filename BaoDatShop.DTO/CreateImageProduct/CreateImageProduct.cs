using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.CreateImageProduct
{
    public class CreateImageProduct
    {
      
        public int ProductId { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; } = true;

    }
}
