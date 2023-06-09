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

        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [DisplayName("Giá (VNĐ)")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        [DefaultValue(0)]
        public int Price { get; set; } = 0;
        public int DiscountId { get; set; }
        public Disscount Disscount { get; set; }
    
        [DisplayName("Tồn kho")]
        [DefaultValue(0)]
        public int Stock { get; set; } = 0;

        public int ProductTypeId { get; set; }

        // Navigation reference property cho khóa ngoại đến ProductType
        [DisplayName("Loại sản phẩm")]
        public ProductTypes ProductType { get; set; }

      
        public string Image { get; set; }

        public bool Status { get; set; } = true;

        // Collection reference property cho khóa ngoại từ Cart
        public List<Cart> Carts { get; set; }

        // Collection reference property cho khóa ngoại từ InvoiceDetail
        public List<InvoiceDetail> InvoiceDetails { get; set; }
        public ICollection<Review> Review { get; set; }
    }
}