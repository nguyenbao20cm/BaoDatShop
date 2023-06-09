using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class Disscount
    {
        public int Id { get; set; } 
        public int NameDisscount { get; set; }
        public ICollection<Product> Product { get; set; }
    }
}
