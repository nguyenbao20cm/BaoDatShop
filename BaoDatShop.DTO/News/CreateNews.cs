using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.News
{
    public class CreateNews
    {
        public string NewsName { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
    }
}
