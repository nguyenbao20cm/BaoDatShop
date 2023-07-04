using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.DTO
{
    public class ApplicationUser : IdentityUser
    {


        public string? Email { get; set; }

     
      
       
        public string Phone { get; set; }

       
        public string Address { get; set; }

    
        public string FullName { get; set; }
       
        public string Avatar { get; set; }
        public bool Status { get; set; } = true;
        public int Permission { get; set; } 
    }
}
