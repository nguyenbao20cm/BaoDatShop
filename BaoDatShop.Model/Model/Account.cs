﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using BaoDatShop.Model.Model;

namespace Eshop.Models
{
    public class Account
    {
        public string Id { get; set; }

        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} từ 6-20 kí tự")]
        public string Username { get; set; }

        

        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "{0} không hợp lệ")]
        public string Email { get; set; }

        [DisplayName("SĐT")]
        [RegularExpression("0\\d{9}", ErrorMessage = "SĐT không hợp lệ")]
        public string Phone { get; set; }

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Họ tên")]
        public string FullName { get; set; }



        [DisplayName("Ảnh đại diện")]
        public string Avatar { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [DisplayName("Còn hoạt động")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        public int Permission { get; set; }  // Quân quyền 1 là admin //2 là Nhân viên kho //3 là khách hàng  //4 là nhân viên bán hàng

       

        //public List<Invoice> Invoices { get; set; }

        // Collection reference property cho khóa ngoại từ Cart
       
        public List<Cart> Carts { get; set; }

    }
}
