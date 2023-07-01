using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Model.Model
{
    public class RoleStaff
    {
        public int Id { get; set; }
        public int RoleStaffDetailId { get; set; }
        public RoleStaffDetail RoleStaffDetail { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

    }
}
