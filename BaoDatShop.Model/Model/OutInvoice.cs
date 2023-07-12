using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class OutInvoice
    {
        public int Id { get; set; }
        public DateTime OutDate { get; set; }
        public int Total { get; set; }
        public int ProductSizeID { get; set; }
        public ProductSize ProductSize { get; set; }
        public int Quanlity { get; set; }
    }
}
