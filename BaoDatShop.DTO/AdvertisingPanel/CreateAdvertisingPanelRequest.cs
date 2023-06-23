using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.AdvertisingPanel
{
    public class CreateAdvertisingPanelRequest
    {
        public int? LinkProduct { get; set; }
        public int? LinkProductType { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }
}
