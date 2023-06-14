using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO.AccountRequest
{
    public class UpdateAccountRequest
    {
        public string Username { get; set; }
       
        public string email { get; set; }
        public string password { get; set; }
        public string newpassword { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public IFormFile image { get; set; }
    }
}
