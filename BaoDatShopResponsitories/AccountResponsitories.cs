using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.LoginRequest;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using BaoDatShop.DTO.Response;
using Microsoft.AspNetCore.Http;
using BaoDatShop.DTO;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Hosting;

namespace BaoDatShop.Responsitories
{
    public interface IAccountResponsitories
    {
        public Task<IdentityResult> SignUp(RegisterRequest model);
        public Task<string> SignIn(LoginModel model);
    }
    public class AccountResponsitories : IAccountResponsitories
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
      public AccountResponsitories
            (UserManager<ApplicationUser> userManager, IWebHostEnvironment _environment,
            SignInManager<ApplicationUser> signInManager,IConfiguration configuration )
        {
            this.configuration = configuration;
            this.userManager=userManager;
            this._environment = _environment;
            this.signInManager = signInManager;
        }
        public async Task<string> SignIn(LoginModel model)
        {
             var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
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
            var  user = new ApplicationUser()
            {
                FullName = model.FullName,
                Address = model.Address,
                Email = model.Email,
                UserName = model.Username,
                Password = model.Password,
                Phone = model.Phone,
                Avatar = fileName,
                Status = true,
                Permission =0,
            };
           return  await userManager.CreateAsync(user, model.Password);
            
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
