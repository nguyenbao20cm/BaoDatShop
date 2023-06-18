using BaoDatShop.DTO;
using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.LoginRequest;
using BaoDatShop.DTO.Role;
using BaoDatShop.Model.Context;
using Eshop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        public Task<IdentityResult> SignUpCustomer(RegisterRequest model);
        public Account GetDetailAccount(string id);
        public Task<string> Update(string id, UpdateAccountRequest model);
        public List<Account> GetAll();
    }
    public class AccountResponsitories : IAccountResponsitories
    {
        private IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountResponsitories
            (UserManager<ApplicationUser> userManager, IWebHostEnvironment _environment,
            AppDbContext context,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHasher
            )
        {
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
            this._environment = _environment;
            this.signInManager = signInManager;
            _roleManager = roleManager;
            this.passwordHasher = passwordHasher;
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
                if (model.image != null)
                {
                    var fileName = id+"jpg";
                    var uploadFolder = Path.Combine(_environment.WebRootPath, "Image", "Avatar");
                    var uploadPath = Path.Combine(uploadFolder, fileName);

                    using (FileStream fs = System.IO.File.Create(uploadPath))
                    {
                        model.image.CopyTo(fs);
                        fs.Flush();
                    }
                    user.Avatar = fileName;
                }
                else
                    return "Failed";

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
                        tamp.Avatar = model.image.FileName;
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
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
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
                Permission = 0,
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
                tamp.Level = 0;
                context.Account.Add(tamp);
                context.SaveChanges();
            }
            return result;
        }
        public async Task<IdentityResult> SignUpAdmin(RegisterRequest model)
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
                Permission = 0,
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
                tamp.Permission = 1;
                tamp.Level = 0;
                context.Account.Add(tamp);
                context.SaveChanges();
            }
            return result;
        }
        public async Task<IdentityResult> SignUpCustomer(RegisterRequest model)
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
                Permission = 0,
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
                tamp.Permission = 3;
                tamp.Level = 1;
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
    }
}
