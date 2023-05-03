using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.News
{
    public class CreateNewsRequest
    {
        public string NewsName { get; set; }

        //  public DateTime DateTime { get; set; }
        public IFormFile Image { get; set; }
        public string Content { get; set; }
    }
}
