using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class AdvertisingPanel
    {
        public int AdvertisingPanelID { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public bool Status { get; set; }

    }
}
