using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int Stock { get; set; } = 0;
        // Navigation reference property cho khóa ngoại đến Product
        public Product Product { get; set; }
        public bool Status { get; set; }
    }
}
