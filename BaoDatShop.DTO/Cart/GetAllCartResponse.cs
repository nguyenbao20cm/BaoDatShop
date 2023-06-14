using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Cart
{
    public class GetAllCartResponse
    {
        public int CartId { get; set; }
        public string AccountId { get; set; }

        public int ProductSizeId { get; set; }

        public int Quantity { get; set; } = 1;
        
    }
}
