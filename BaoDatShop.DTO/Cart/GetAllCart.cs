﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Cart
{
    public class GetAllCart
    {
        public int CartId { get; set; }
        public string AccountId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
