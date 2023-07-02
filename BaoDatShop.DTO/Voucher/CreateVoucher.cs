using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.Voucher
{
    public class CreateVoucher
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public int Disscount { get; set; }
        public bool Status { get; set; }
    }
}
