using BLL.Request;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DLL.Model;
using DLL.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Utility.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
  public interface IAccountService
    {
        Task<string> LoginUser(LoginRequest request);
        Task<string> Test(ClaimsPrincipal tt);
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;

        public AccountService(UserManager<AppUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        public async Task<string> LoginUser(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new MyApplicationExceptions("User Not found");
            }

            var matchUser = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!matchUser)
            {
                throw new MyApplicationExceptions("User name and password does not match");
            }

            return await GenerateJSONWebToken(user);
        }

        public Task<string> Test(ClaimsPrincipal tt)
        {
            var userId = tt.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            var userName = tt.Claims.FirstOrDefault(x => x.Type == "userName")?.Value; 
            var role = tt.FindFirst(ClaimTypes.Role)?.Value;
            var email = tt.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();

            throw new NotImplementedException();
        }

        private async Task<string> GenerateJSONWebToken(AppUser userInfo)
        {
            var userRole = (await _userManager.GetRolesAsync(userInfo)).FirstOrDefault();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Id.ToString()),
                new Claim(CustomJwtClaimsName.UserName, userInfo.UserName.ToString()),
                new Claim(CustomJwtClaimsName.Email, userInfo.Email.ToString()),
                new Claim(ClaimTypes.Role, userRole),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
