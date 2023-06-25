using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class HistoryAccount
    {
        public int Id { get; set; }
        public string AccountID { get; set; }
        public Account Account { get; set; }
        public string Content { get; set; }
        public DateTime Datetime { get; set; }

    }
}
