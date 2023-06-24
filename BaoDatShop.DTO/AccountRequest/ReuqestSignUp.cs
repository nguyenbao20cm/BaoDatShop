using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.AccountRequest
{
    public class ReuqestSignUp
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        [DisplayName("SĐT")]
        [RegularExpression("0\\d{9}", ErrorMessage = "SĐT không hợp lệ")]
        public string Phone { get; set; }

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Họ tên")]
        public string FullName { get; set; }
        [DisplayName("Ảnh đại diện")]
     
        
        public string image { get; set; }
        public bool status { get; set; }
        //public bool Status { get; set; } = true;
        //public int Permission { get; set; } = false;
    }
}
