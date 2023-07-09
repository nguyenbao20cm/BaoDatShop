using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.LoginRequest;
using BaoDatShop.Responsitories;
using Eshop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BaoDatShop.Service
{
    public interface IAccountService
    {
        public Task<string> UpdateAccountCustomer(string id, UpdateAccountCustomerRequest model);
        public Task<string> Update(string id,UpdateAccountRequest model);
        public Task<string> SignIn(LoginRequest model);
        public Task<IdentityResult> SignUp(RegisterRequest model);
        public Task<IdentityResult> SignUpAdmin(RegisterRequest model);
        public Task<IdentityResult> SignUpCustomer(ReuqestSignUp model);
        public Account GetDetailAccount(string id);
        public List<Account> GetAllAccount();
        public Task<string> DeleteAccount(string idacc,string id);
        public List<Account> GetAllAcountCustomer();
        public Task<IdentityResult> RegisterStaff(ReuqestSignUp model);
        public bool CreateAvatarImage( IFormFile model);
        public Task<bool> ActiveAccount(string idacc,string id);

    }
    public class AccountService : IAccountService
    {
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        private readonly IAccountResponsitories accountResponsitories;
        public AccountService(IAccountResponsitories accountResponsitories, IHistoryAccountResponsitories iHistoryAccountResponsitories)
        {
            this.accountResponsitories = accountResponsitories;
            IHistoryAccountResponsitories = iHistoryAccountResponsitories;  
        }

        public async Task<bool> ActiveAccount(string idacc,string id)
        {
            return await accountResponsitories.ActiveAccount(idacc,id);
        }

        public bool CreateAvatarImage( IFormFile model)
        {
          
            return  accountResponsitories.CreateAvatarImage(model);
        } 

        public Task<string> DeleteAccount(string idacc,string id)
        {
            return accountResponsitories.DeleteAccount(idacc,id);
        }

        public List<Account> GetAllAccount()
        {
            return accountResponsitories.GetAll();
        }

        public List<Account> GetAllAcountCustomer()
        {
            return accountResponsitories.GetAll().Where(a=>a.Permission==3).ToList();
        }

        public Account GetDetailAccount(string id)
        {
            return accountResponsitories.GetDetailAccount(id);
        }

        public Task<IdentityResult> RegisterStaff(ReuqestSignUp model)
        {
            return accountResponsitories.RegisterStaff(model);
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
        public Task<IdentityResult> SignUpCustomer(ReuqestSignUp model)
        {
            return accountResponsitories.SignUpCustomer(model);
        }

        public Task<string> Update(string id, UpdateAccountRequest model)
        {
            return accountResponsitories.Update(id, model);
        }

        public Task<string> UpdateAccountCustomer(string id, UpdateAccountCustomerRequest model)
        {
            return accountResponsitories.UpdateAccountCustomer(id, model);
        }
    }
}
