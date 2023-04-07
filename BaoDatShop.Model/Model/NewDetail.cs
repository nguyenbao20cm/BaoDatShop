using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class NewDetail
    {
        public int NewDetailId { get; set; }
        public int NewId { get; set; }
        public News New { get; set; }
        public string Image { get; set; }
    }
}
