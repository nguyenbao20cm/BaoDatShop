using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Invoice
{
    public class Report
    {
        public DateTime DateTime { get; set; }
        public int Total { get; set; }
        public int TotalImport { get; set; }
    }
}
