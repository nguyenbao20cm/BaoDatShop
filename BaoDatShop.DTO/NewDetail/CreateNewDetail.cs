using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.NewDetail
{
    public class CreateNewDetail
    {
        public int NewId { get; set; }
    
        public IFormFile Image { get; set; }
    }
}
