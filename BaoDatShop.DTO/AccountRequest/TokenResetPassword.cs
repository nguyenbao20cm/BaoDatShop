﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.AccountRequest
{
    public class TokenResetPassword
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
