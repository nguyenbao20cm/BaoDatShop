using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.ProductSize
{
    public class CreateProductSize
    {
        public int Stock { get; set; } = 0;
        public string Name { get; set; }
        public int ProductId { get; set; }
        public string Status { get; set; }
    }
}

