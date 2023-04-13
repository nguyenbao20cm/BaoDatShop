using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class Review
    {
        public int ReviewId { get; set; } 
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
        public int Star { get; set; }
        public bool Status { get; set; }
    }
}
