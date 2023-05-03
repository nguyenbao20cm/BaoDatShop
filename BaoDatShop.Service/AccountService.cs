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
        public Task<string> SignIn(LoginRequest model);
        public Task<IdentityResult> SignUp(RegisterRequest model);
        public Task<IdentityResult> SignUpAdmin(RegisterRequest model);
        public Task<IdentityResult> SignUpCustomer(RegisterRequest model);
    }
    public class AccountService:IAccountService
    {

        private readonly IAccountResponsitories accountResponsitories;
        public AccountService(IAccountResponsitories accountResponsitories)
        {
            this.accountResponsitories=accountResponsitories; 
        }

        public Task<string> SignIn(LoginRequest model)
        {
            return accountResponsitories.SignIn(model);
        }

        public Task<IdentityResult> SignUp(RegisterRequest model)
        {
            return accountResponsitories.SignUp(model);
        }

        public Task<IdentityResult> SignUpAdmin(RegisterRequest model)
        {
            return accountResponsitories.SignUpAdmin(model);
        }
        public Task<IdentityResult> SignUpCustomer(RegisterRequest model)
        {
            return accountResponsitories.SignUpCustomer(model);
        }
    }
}
