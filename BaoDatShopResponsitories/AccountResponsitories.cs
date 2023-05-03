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
using System.Security.Claims;
using System.Text;

namespace BaoDatShop.Responsitories
{
    public interface IAccountResponsitories
    {
        public Task<IdentityResult> SignUp(RegisterRequest model);
        public Task<IdentityResult> SignUpAdmin(RegisterRequest model);
        public Task<string> SignIn(LoginRequest model);
        public Task<IdentityResult> SignUpCustomer(RegisterRequest model);

    }
    public class AccountResponsitories : IAccountResponsitories
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountResponsitories
            (UserManager<ApplicationUser> userManager, IWebHostEnvironment _environment,
            AppDbContext context,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
            this._environment = _environment;
            this.signInManager = signInManager;
            _roleManager = roleManager;
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
                }

                var token = GetToken(authClaims);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return String.Empty;

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
                Password = model.Password,
                Phone = model.Phone,
                Avatar = fileName,
                Status = true,
                Permission = 0,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var user1 = await userManager.FindByNameAsync(model.Username);
                Account tamp = new();
                tamp.Id = user1.Id;
                tamp.FullName = model.FullName;
                tamp.Address = model.Address;
                tamp.Email = model.Email;
                tamp.Username = model.Username;
                tamp.Password = model.Password;
                tamp.Phone = model.Phone;
                tamp.Avatar = fileName;
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
                Password = model.Password,
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
                Account tamp = new();
                tamp.Id = user1.Id;
                tamp.FullName = model.FullName;
                tamp.Address = model.Address;
                tamp.Email = model.Email;
                tamp.Username = model.Username;
                tamp.Password = model.Password;
                tamp.Phone = model.Phone;
                tamp.Avatar = fileName;
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
                Password = model.Password,
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
                Account tamp = new();
                tamp.Id = user1.Id;
                tamp.FullName = model.FullName;
                tamp.Address = model.Address;
                tamp.Email = model.Email;
                tamp.Username = model.Username;
                tamp.Password = model.Password;
                tamp.Phone = model.Phone;
                tamp.Avatar = fileName;
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
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
