using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class News
    {
        public int NewsId { get; set; }
        public string NewsName { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public ICollection<NewDetail> NewDetail { get; set; }
    }
}
