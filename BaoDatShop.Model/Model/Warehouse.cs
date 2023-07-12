using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class Warehouse
    {
        public int Id { get; set; }
        public ProductSize ProductSize { get; set; }
        public int ProductSizeId { get; set; }
    
        public int Stock { get; set; }
    }
}
