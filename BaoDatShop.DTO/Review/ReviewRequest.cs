using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Review
{
    public class ReviewRequest
    {
        public int ProductId { get; set; }
        public string Content { get; set; }
        //public DateTime DateTime { get; set; }
        public int Star { get; set; }
    }
}
