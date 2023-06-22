using BaoDatShop.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Models
{
    public class Product
    {
        public int Id { get; set; }
       
        public string SKU { get; set; }
     
        public string Name { get; set; }

     
        public string Description { get; set; }

    
        public int Price { get; set; } = 0;
    
        public int ProductTypeId { get; set; }

       
        public ProductTypes ProductType { get; set; }

      
        public string Image { get; set; }
      
        public bool Status { get; set; } = true;

        public int CountSell { get; set; }


        //public List<InvoiceDetail> InvoiceDetails { get; set; }
      
        
    }
}