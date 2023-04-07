using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.LoginRequest;
using BaoDatShop.Responsitories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IAccountService
    {
        public Task<string> SignIn(LoginModel model);
        public Task<IdentityResult> SignUp(RegisterRequest model); 
    }
    public class AccountService:IAccountService
    {

        private readonly IAccountResponsitories accountResponsitories;
        public AccountService(IAccountResponsitories accountResponsitories)
        {
            this.accountResponsitories=accountResponsitories; 
        }

        public Task<string> SignIn(LoginModel model)
        {
            return accountResponsitories.SignIn(model);
        }

        public Task<IdentityResult> SignUp(RegisterRequest model)
        {
            return accountResponsitories.SignUp(model);
        }
    }
}
