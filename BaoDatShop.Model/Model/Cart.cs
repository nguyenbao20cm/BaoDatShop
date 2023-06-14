using BaoDatShop.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductSizeId { get; set; }
        public ProductSize ProductSize { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        //public int ProductId { get; set; }
        //public Product Product { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
