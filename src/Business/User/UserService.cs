
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.User
{

    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;  


        public UserService(UserManager<IdentityUser> userManager,
                            SignInManager<IdentityUser> signInManager,
                            IConfiguration configuration,
                            IHttpContextAccessor httpContextAccessor,
                            ILogger<UserService> logger)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<string> CreateUserAsync(CreateUserRequestModel createUserRequestModel)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = createUserRequestModel.Username,
                    Email = createUserRequestModel.Email,
                };

                var result = await _userManager.CreateAsync(user, createUserRequestModel.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return $"User creation failed: {errors}";
                }

                return "User created successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user.");
                return $"An error occurred while creating the user: {ex.Message}";
            }
        }
        public async Task<string> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                    return "Invalid credentials.";

                var accessTokenExpiryInSeconds = 1800;
                var refreshTokenExpiryInSeconds = 86400;

                var accessToken = GenerateJwtToken(user, accessTokenExpiryInSeconds);
                var refreshToken = Guid.NewGuid().ToString();

                SetAuthCookiesInClient(accessToken, accessTokenExpiryInSeconds, user.UserName, refreshToken, refreshTokenExpiryInSeconds);

                return accessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                return $"An error occurred during login: {ex.Message}";
            }
        }


        private string GenerateJwtToken(IdentityUser user, int expiryInSeconds)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                     new Claim(ClaimTypes.Name, user.UserName),
                 }),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddSeconds(expiryInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private void SetAuthCookiesInClient(string accessToken, int accessTokenExpiryInSeconds, string userName, string refreshToken, int refreshTokenExpiryInSeconds)
        {
            var expiryAccess = DateTimeOffset.UtcNow.AddSeconds(accessTokenExpiryInSeconds);
            var expiryRefresh = DateTimeOffset.UtcNow.AddSeconds(refreshTokenExpiryInSeconds);

            var response = _httpContextAccessor.HttpContext.Response;

            response.Cookies.Append("X-Access-Token", Uri.EscapeDataString(accessToken),
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = expiryAccess
                });

            response.Cookies.Append("X-Access-Token-ExpiryInSeconds", accessTokenExpiryInSeconds.ToString(),
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = expiryAccess
                });

            response.Cookies.Append("X-Username", userName,
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = expiryRefresh
                });

            response.Cookies.Append("X-Refresh-Token", Uri.EscapeDataString(refreshToken),
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = expiryRefresh
                });

            response.Cookies.Append("X-Refresh-ExpiryInSeconds", refreshTokenExpiryInSeconds.ToString(),
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = expiryRefresh
                });
        }

    }
}
