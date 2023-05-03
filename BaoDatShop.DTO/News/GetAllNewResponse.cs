using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.News
{
    public class GetAllNewResponse
    {
        public int NewsId { get; set; }
        public string NewsName { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }
}
