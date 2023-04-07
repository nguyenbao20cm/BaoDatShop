using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.DTO.LoginRequest;
using BaoDatShop.DTO.Response;
using BaoDatShop.DTO.Role;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BaoDatShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        
        private readonly IConfiguration _configuration;
        public AccountController(
            IAccountService accountService,
        
           IConfiguration configuration)
        {
          
            _configuration = configuration;
            _accountService = accountService;
        }
        [HttpPost("SignupUser")]
        public async Task<IActionResult> signup([FromForm] RegisterRequest model)
        {
            var result = await _accountService.SignUp(model);
            if (result.Succeeded) return Ok(result.Succeeded);
            return Unauthorized();
        }
        [HttpPost("Signin")]
        public async Task<IActionResult> signin(LoginModel model)
        {
            var result = await _accountService.SignIn(model);
            if (string.IsNullOrEmpty( result)) return Unauthorized();
            return Ok(result);
        }

        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Username);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);

        //        var authClaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        };

        //        foreach (var userRole in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //        }

        //        var token = GetToken(authClaims);

        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token),
        //            expiration = token.ValidTo
        //        });
        //    }
        //    return Unauthorized();
        //}

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        //{
        //    var userExists = await _userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

        //    IdentityUser user = new()
        //    {
        //        Email = model.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Username
        //    };
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        //    return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        //}

        //[HttpPost]
        //[Route("register-Customer")]
        //public async Task<IActionResult> RegisterCustomer([FromBody] RegisterRequest model)
        //{
        //    var userExists = await _userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

        //    IdentityUser user = new()
        //    {
        //        Email = model.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Username
        //    };
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        //    if (!await _roleManager.RoleExistsAsync(UserRole.Costumer))
        //        await _roleManager.CreateAsync(new IdentityRole(UserRole.Costumer));


        //    if (await _roleManager.RoleExistsAsync(UserRole.Costumer))
        //    {
        //        await _userManager.AddToRoleAsync(user, UserRole.Costumer);
        //    }

        //    return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        //}
        //[HttpPost]
        //[Route("register-Admin")]
        //public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest model)
        //{
        //    var userExists = await _userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

        //    IdentityUser user = new()
        //    {
        //        Email = model.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Username
        //    };
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        //    if (!await _roleManager.RoleExistsAsync(UserRole.Admin))
        //        await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
        //    if (!await _roleManager.RoleExistsAsync(UserRole.User))
        //        await _roleManager.CreateAsync(new IdentityRole(UserRole.User));

        //    if (await _roleManager.RoleExistsAsync(UserRole.Admin))
        //    {
        //        await _userManager.AddToRoleAsync(user, UserRole.Admin);
        //    }
        //    if (await _roleManager.RoleExistsAsync(UserRole.Admin))
        //    {
        //        await _userManager.AddToRoleAsync(user, UserRole.User);
        //    }
        //    return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        //}

        //private JwtSecurityToken GetToken(List<Claim> authClaims)
        //{
        //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["JWT:ValidIssuer"],
        //        audience: _configuration["JWT:ValidAudience"],
        //        expires: DateTime.Now.AddHours(3),
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //        );

        //    return token;
        //}
    }
}
