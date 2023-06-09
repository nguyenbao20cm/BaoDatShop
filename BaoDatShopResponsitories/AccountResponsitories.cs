﻿using BaoDatShop.DTO;
using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.LoginRequest;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using BaoDatShop.Model.Model;
using Eshop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace BaoDatShop.Responsitories
{
    public interface IAccountResponsitories
    {
        public Task<IdentityResult> SignUp(RegisterRequest model);
        public Task<IdentityResult> SignUpAdmin(RegisterRequest model);
        public Task<string> SignIn(LoginRequest model);
        public Task<IdentityResult> SignUpCustomer(ReuqestSignUp model);
        public Account GetDetailAccount(string id);
        public Task<string> Update(string id, UpdateAccountRequest model);
        public Task<string> UpdateAccountCustomer(string id, UpdateAccountCustomerRequest model);
        public List<Account> GetAll();
        public Task<string> DeleteAccount(string idac,string id);
        public Task<IdentityResult> RegisterStaff(ReuqestSignUp model);
        public bool CreateAvatarImage(  IFormFile model);
        public  Task<bool> ActiveAccount(string idacc,string id);
        public Task<IdentityResult> RegisterStaffKHO(ReuqestSignUp model);
        
    }

    public class AccountResponsitories : IAccountResponsitories
    {
        private readonly IHistoryAccountResponsitories IHistoryAccountResponsitories;
        private IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountResponsitories
            (UserManager<ApplicationUser> userManager,
            IHistoryAccountResponsitories IHistoryAccountResponsitories,
        IWebHostEnvironment _environment,
        AppDbContext context,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHasher
            )
        {
            this.IHistoryAccountResponsitories = IHistoryAccountResponsitories;
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
            this._environment = _environment;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            this.passwordHasher = passwordHasher;
        }
        public async Task<string> DeleteAccount(string idacc,string id)
        {
           
            try {
                var user = await userManager.FindByIdAsync(id);
                if (user == null) return "Thất bại";
                user.Status = false;
                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    Account tamp = context.Account.Where(a => a.Id == id).FirstOrDefault();
                    tamp.Status = false;
                    context.Account.Update(tamp);
                    int check= context.SaveChanges();
                    if(check>0)
                    {
                        HistoryAccount a = new();
                        a.AccountID = idacc; a.Datetime = DateTime.Now;
                        a.Content = "Đã khóa tài khoản UserName" + user.UserName;
                        IHistoryAccountResponsitories.Create(a);
                        return "Thành công";
                    }    
                    else
                        return "Thất bại";
                }
                else
                    return "Thất bại";
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} First exception caught.", ex.Message);
            }
            return "Thất bại";

        }
        public async Task<string> UpdateAccountCustomer(string id, UpdateAccountCustomerRequest model)
        {

            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(model.FullName))
                    user.FullName = model.FullName;
                else
                    return "Failed";
                if (!string.IsNullOrEmpty(model.Address))
                    user.Address = model.Address;
                else
                    return "Failed";
                if (!string.IsNullOrEmpty(model.Phone))
                    user.Phone = model.Phone;
                else
                    return "Failed";
                if(model.Phone!=user.Phone)
                { 
                    foreach(var a in context.Account)
                    {
                        if(a.Phone == model.Phone) return "SDT";// SDT bij trungf
                    }
                 }
                IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        Account tamp = context.Account.Where(a => a.Id == id).FirstOrDefault();
                        tamp.FullName = model.FullName;
                        tamp.Address = model.Address;
                        tamp.Phone = model.Phone;
                    tamp.Avatar = id+".jpg";
                    context.Account.Update(tamp);
                        context.SaveChanges();
                        return "True";
                    }
                    else
                        return "Failed";
            }
            return "Failed";
        }
        public async Task<string> Update(string id, UpdateAccountRequest model)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if(await userManager.CheckPasswordAsync(user, model.password)!=true)
                   return "Failed";
                if (!string.IsNullOrEmpty(model.email))
                    user.Email = model.email;
                else
                    return "Failed";
                if (!string.IsNullOrEmpty(model.newpassword))
                    user.PasswordHash = passwordHasher.HashPassword(user, model.newpassword);
                else
                    return "Failed";
                if (!string.IsNullOrEmpty(model.Username))
                    user.UserName = model.Username;
                else
                    return "Failed";
                if (!string.IsNullOrEmpty(model.FullName))
                    user.FullName =  model.FullName;
                else
                    return "Failed";
                if (!string.IsNullOrEmpty(model.Address))
                    user.Address = model.Address;
                else
                    return "Failed";
                if (!string.IsNullOrEmpty(model.Phone))
                    user.Phone =  model.Phone;
                else
                    return "Failed";
                //if (model.image != null)
                //{
                //    var fileName = id+"jpg";
                //    var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Avatar");
                //    var uploadPath = Path.Combine(uploadFolder, fileName);

                //    using (FileStream fs = System.IO.File.Create(uploadPath))
                //    {
                //        model.image.CopyTo(fs);
                //        fs.Flush();
                //    }
                //    user.Avatar = fileName;
                //}
                //else
                //    return "Failed";

                if (!string.IsNullOrEmpty(model.email) && !string.IsNullOrEmpty(model.newpassword))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                         
                        Account tamp = context.Account.Where(a => a.Id == id).FirstOrDefault();
                        tamp.FullName = model.FullName;
                        tamp.Address = model.Address;
                        tamp.Email = model.email;
                        tamp.Username = model.Username;
                        tamp.Phone = model.Phone;
                        
                        context.Account.Update(tamp);
                        context.SaveChanges();
                        return "True";
                    }
                    else
                        return "Failed";
                }
            }
            return "Failed";
        }
        public Account GetDetailAccount(string id)
        {
            return context.Account.Where(a => a.Id == id).FirstOrDefault();
        }

        public async Task<string> SignIn(LoginRequest model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return "Thất bại";
           var tam= await userManager.IsEmailConfirmedAsync(user);
            if(tam==false) return "Chưa xác minh Email";
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                if(user.Status==false) return "Người dùng đã bị khóa";
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    authClaims.Add(new Claim("RoleUser", userRole));
                }

                var token = GetToken(authClaims);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return "Failed";

        }

        public async Task<IdentityResult> SignUp(RegisterRequest model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            var fileName = model.image.FileName;
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Avatar");
            var uploadPath = Path.Combine(uploadFolder, fileName);

            using (FileStream fs = System.IO.File.Create(uploadPath))
            {
                model.image.CopyTo(fs);
                fs.Flush();
            }
            var user = new ApplicationUser()
            {
                FullName = model.FullName,
                Address = model.Address,
                Email = model.Email,
                UserName = model.Username,
                Phone = model.Phone,
                Avatar = fileName,
                Status = true,
                Permission = 2,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var user1 = await userManager.FindByNameAsync(model.Username);
                user1.Avatar = user1.Id + "jpg";
                await userManager.UpdateAsync(user1);
                Account tamp = new();
                tamp.Id = user1.Id;
                tamp.FullName = model.FullName;
                tamp.Address = model.Address;
                tamp.Email = model.Email;
                tamp.Username = model.Username;
              
                tamp.Phone = model.Phone;
                tamp.Avatar = user1.Id + "jpg";
                tamp.Status = true;
                tamp.Permission = 2;
              
                context.Account.Add(tamp);
                context.SaveChanges();
            }
            return result;
        }
        public async Task<IdentityResult> SignUpAdmin(RegisterRequest model)
        {
          
            var userExists = await userManager.FindByNameAsync(model.Username);
        
            var user = new ApplicationUser()
            {
                FullName = model.FullName,
                Address = model.Address,
                Email = model.Email,
                UserName = model.Username,
               
                Phone = model.Phone,
                Avatar = "",
                Status = true,
                Permission = 1,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!await _roleManager.RoleExistsAsync(UserRole.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
           
            if (await _roleManager.RoleExistsAsync(UserRole.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRole.Admin);
            }
           
            if (result.Succeeded)
            {
                var user1 = await userManager.FindByNameAsync(model.Username);
                user1.Avatar = user1.Id + ".jpg";
                await userManager.UpdateAsync(user1);
                Account tamp = new();
                tamp.Id = user1.Id;
                tamp.FullName = model.FullName;
                tamp.Address = model.Address;
                tamp.Email = model.Email;
                tamp.Username = model.Username;
             
                tamp.Phone = model.Phone;
                tamp.Avatar = user1.Id + ".jpg";
                tamp.Status = true;
                tamp.Permission = 1;
          
                context.Account.Add(tamp);
                context.SaveChanges();
                var fileName = user1.Id + ".jpg";
                var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Avatar");
                var uploadPath = Path.Combine(uploadFolder, fileName);

                using (FileStream fs = System.IO.File.Create(uploadPath))
                {
                    model.image.CopyTo(fs);
                    fs.Flush();
                }
            }
            return result;
        }
        public async Task<IdentityResult> SignUpCustomer(ReuqestSignUp model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            //var fileName = model.image.FileName;
            //var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Avatar");
            //var uploadPath = Path.Combine(uploadFolder, fileName);

            //using (FileStream fs = System.IO.File.Create(uploadPath))
            //{
            //    model.image.CopyTo(fs);
            //    fs.Flush();
            //}
            var user = new ApplicationUser()
            {
                FullName = model.FullName,
                Address = model.Address,
                Email = model.Email,
                UserName = model.Username,
              
                Phone = model.Phone,
                Avatar = model.image,
                Status = true,
                Permission = 3,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!await _roleManager.RoleExistsAsync(UserRole.Costumer))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Costumer));


            if (await _roleManager.RoleExistsAsync(UserRole.Costumer))
            {
                await userManager.AddToRoleAsync(user, UserRole.Costumer);
            }
            if (result.Succeeded)
            {
                var user1 = await userManager.FindByNameAsync(model.Username);
                user1.Avatar = user1.Id + ".jpg";
                await userManager.UpdateAsync(user1);
                Account tamp = new();
                tamp.Id = user1.Id;
                tamp.FullName = model.FullName;
                tamp.Address = model.Address;
                tamp.Email = model.Email;
                tamp.Username = model.Username;
              
                tamp.Phone = model.Phone;
                tamp.Avatar = user1.Id + ".jpg";
                tamp.Status = true;
                tamp.Permission = 3;
               
                context.Account.Add(tamp);
                context.SaveChanges();
            }
            return result;
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        public List<Account> GetAll()
        {
            return context.Account.ToList();
        }
        public async Task<IdentityResult> RegisterStaffKHO(ReuqestSignUp model)//QL KHO
        {
            //var userExists = await userManager.FindByNameAsync(model.Username);
            //var fileName = model.image.FileName;
            //var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Avatar");
            //var uploadPath = Path.Combine(uploadFolder, fileName);

            //using (FileStream fs = System.IO.File.Create(uploadPath))
            //{
            //    model.image.CopyTo(fs);
            //    fs.Flush();
            //}
            var user = new ApplicationUser()
            {
                FullName = model.FullName,
                Address = model.Address,
                Email = model.Email,
                UserName = model.Username,
                Phone = model.Phone,
                Avatar = model.image,
                Status = model.status,
                Permission = 4,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            //if (!await _roleManager.RoleExistsAsync(UserRole.Staff))
            //    await _roleManager.CreateAsync(new IdentityRole(UserRole.Staff));

            //if (await _roleManager.RoleExistsAsync(UserRole.Staff))
            //{
            //    await userManager.AddToRoleAsync(user, UserRole.Staff);
            //}
            if (!await _roleManager.RoleExistsAsync(UserRole.StaffKHO))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.StaffKHO));

            if (await _roleManager.RoleExistsAsync(UserRole.StaffKHO))
            {
                await userManager.AddToRoleAsync(user, UserRole.StaffKHO);
            }
            if (result.Succeeded)
            {
                var user1 = await userManager.FindByNameAsync(model.Username);
                user1.Avatar = user1.Id + ".jpg";
                await userManager.UpdateAsync(user1);
                Account tamp = new();
                tamp.Id = user1.Id;
                tamp.FullName = model.FullName;
                tamp.Address = model.Address;
                tamp.Email = model.Email;
                tamp.Username = model.Username;
                tamp.Phone = model.Phone;
                tamp.Avatar = user1.Id + ".jpg";
                tamp.Status = model.status;
                tamp.Permission = 4;

                context.Account.Add(tamp);
                context.SaveChanges();
            }
            return result;
        }
        public async Task<IdentityResult> RegisterStaff(ReuqestSignUp model)//QL BANHANG
        {
            //var userExists = await userManager.FindByNameAsync(model.Username);
            //var fileName = model.image.FileName;
            //var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Avatar");
            //var uploadPath = Path.Combine(uploadFolder, fileName);

            //using (FileStream fs = System.IO.File.Create(uploadPath))
            //{
            //    model.image.CopyTo(fs);
            //    fs.Flush();
            //}
            var user = new ApplicationUser()
            {
                FullName = model.FullName,
                Address = model.Address,
                Email = model.Email,
                UserName = model.Username,
                Phone = model.Phone,
                Avatar = model.image,
                Status = model.status,
                Permission = 4,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            //if (!await _roleManager.RoleExistsAsync(UserRole.Staff))
            //    await _roleManager.CreateAsync(new IdentityRole(UserRole.Staff));

            //if (await _roleManager.RoleExistsAsync(UserRole.Staff))
            //{
            //    await userManager.AddToRoleAsync(user, UserRole.Staff);
            //}
            if (!await _roleManager.RoleExistsAsync(UserRole.Staff))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Staff));

            if (await _roleManager.RoleExistsAsync(UserRole.Staff))
            {
                await userManager.AddToRoleAsync(user, UserRole.Staff);
            }
            if (result.Succeeded)
            {
                var user1 = await userManager.FindByNameAsync(model.Username);
                user1.Avatar = user1.Id + ".jpg";
                await userManager.UpdateAsync(user1);
                Account tamp = new();
                tamp.Id = user1.Id;
                tamp.FullName = model.FullName;
                tamp.Address = model.Address;
                tamp.Email = model.Email;
                tamp.Username = model.Username;
                tamp.Phone = model.Phone;
                tamp.Avatar = user1.Id + ".jpg";
                tamp.Status = model.status ;
                tamp.Permission = 4;
                
                context.Account.Add(tamp);
                context.SaveChanges();
            }
            return result;
        }

        private async Task<string> Check(string name)
        {
            var userExists = await userManager.FindByNameAsync(name);
            if (userExists == null) return "Thất bại";
            var fileName = userExists.Id;
            return fileName;

        }
        public  bool CreateAvatarImage(IFormFile model)
        {
            var a = Check(model.FileName).Result;
            var fileName = a+".jpg";
            var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Avatar");
            var uploadPath = Path.Combine(uploadFolder, fileName);
            try
            {
                using (FileStream fs = System.IO.File.Create(uploadPath))
                {
                    model.CopyTo(fs);
                    fs.Flush();
                }
            }
            catch (Exception e)
            {
                
            }
            return true;
         
        }

        public async Task<bool> ActiveAccount(string idacc,string id)
        {
            var user = await userManager.FindByIdAsync(id);
            user.Status = true;
              await  userManager.UpdateAsync(user);
            var a = context.Account.Where(a => a.Id == id).FirstOrDefault();
            a.Status = true;
            context.Account.Update(a);
            int check = context.SaveChanges();
             if (check > 0)
            {
                HistoryAccount ab = new();
                ab.AccountID = idacc; ab.Datetime = DateTime.Now;
                ab.Content = "Đã khóa tài khoản UserName" + user.UserName;
                IHistoryAccountResponsitories.Create(ab);
                return  true;
            }
            else
                return false;
       
        }
    }
}
   