using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class Voucher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
     
        public int Disscount { get; set; }
        public bool Status { get; set; }
    }
}
