﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO
{
    public class FooterRequest
    {
        public IFormFile Avatar { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string LinkFacebook { get; set; }
        public string LinkInstagram { get; set; }
        public string LinkZalo { get; set; }
    }
}
