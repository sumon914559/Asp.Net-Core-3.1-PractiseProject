using BLL.Request;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BLL.Response;
using DLL.Model;
using DLL.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Utility.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Utility.Helpers;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace BLL.Services
{
  public interface IAccountService
    {
        Task<LoginResponse> LoginUser(LoginRequest request);
        Task<string> Test(ClaimsPrincipal tt);
        Task<SuccessResponse> LogOut(ClaimsPrincipal tt);
        Task<LoginResponse> RefreshToken(string refreshToken);
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly TaposRSA _taposRsa;
        private readonly IDistributedCache _cache;

        public AccountService(UserManager<AppUser> userManager, IConfiguration config,TaposRSA taposRsa,IDistributedCache cache)
        {
            _userManager = userManager;
            _config = config;
            _taposRsa = taposRsa;
            _cache = cache;
        }
        public async Task<LoginResponse> LoginUser(LoginRequest request)
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

        public async Task<SuccessResponse> LogOut(ClaimsPrincipal tt)
        {
            var userId = tt.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            var accessTokenKey = userId + "_accesstoken";
            var refreshTokenKey = userId + "_refreshtoken";
            await _cache.RemoveAsync(accessTokenKey);
            await _cache.RemoveAsync(refreshTokenKey);
            return new SuccessResponse()
            {
                Message = "Log Out successfull",
                statusCode = 200
            };
        }

        public async Task<LoginResponse> RefreshToken(string refreshToken)
        {
            var decryptRsa = _taposRsa.Decrypt(refreshToken, "v1");
            if (decryptRsa == null)
            {
                throw new MyApplicationExceptions("Refresh Token Not found");
            }

            var refreshTokenObject = JsonConvert.DeserializeObject<RefreshTokenResponse>(decryptRsa);
            var refreshTokenKey = refreshTokenObject.UserId+ "_refreshtoken";
            var cacheData =  await _cache.GetStringAsync(refreshTokenKey);
            if (cacheData == null)
            {
                throw new MyApplicationExceptions("Refresh Token Not found");
            }

            if (cacheData != refreshToken)
            {
                throw new MyApplicationExceptions("Refresh Token Not found");
            }

            var user = await _userManager.FindByIdAsync(refreshTokenObject.UserId.ToString());
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

        private async Task<LoginResponse> GenerateJSONWebToken(AppUser userInfo)
        {
            var response = new LoginResponse();
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
            var times = _config.GetValue<int>("Jwt:AccessTokenLifeTime");
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(times),
                signingCredentials: credentials);

            var refreshToken = new RefreshTokenResponse()
            {
                id = Guid.NewGuid().ToString(),
                UserId = userInfo.Id

            };

            var rseData = _taposRsa.EncryptData(JsonConvert.SerializeObject(refreshToken),"v1");

            response.Token =  new JwtSecurityTokenHandler().WriteToken(token);
            response.Expire = times * 60;
            response.RefreshToken = rseData;
            await StoreTokenInformaton(userInfo.Id, response.Token, response.RefreshToken);
            return response;
        }

        private async Task StoreTokenInformaton(long userId, string accessToken, string refreshToken)
        {
           

            var accessTokenOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_config.GetValue<int>("Jwt:AccessTokenLifeTime")));

            var refreshTokenOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_config.GetValue<int>("Jwt:RefreshTokenLifeTime")));

            var accessTokenKey = userId.ToString() + "_accesstoken";
            var refreshTokenKey = userId.ToString() + "_refreshtoken";

            await _cache.SetStringAsync(accessTokenKey, accessToken, accessTokenOptions);

            await _cache.SetStringAsync(refreshTokenKey, refreshToken, refreshTokenOptions);
        }
    }
}
