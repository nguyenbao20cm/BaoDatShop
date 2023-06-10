using Eshop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class Disscount
    {
        public int Id { get; set; } 
        public int NameDisscount { get; set; }
        public int ProductId { get; set; }

        // Navigation reference property cho khóa ngoại đến Product
        public Product Product { get; set; }
        public bool Status { get; set; }
     
    }
}
