using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Cart
{
    public class CreateCart
    {
        public int AccountId { get; set; }

        public int ProductId { get; set; }
   
        public int Quantity { get; set; } = 1;
    }
}
