
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentClaimApi.Data;


namespace PaymentClaimApi
{
    
[Route("api/[controller]")]
[ApiController]
public class JWTTokenController : ControllerBase
{
    
    public IConfiguration _configuration;
    public readonly ApplicationDbContext _context;

    public JWTTokenController(IConfiguration configuration,ApplicationDbContext context)
    {
        _context = context;
        _configuration = configuration;
    }


        [HttpPost]

        public async Task<IActionResult> PostAsync(User user)
        {
            if (user != null && user.UserName != null && user.Password != null)
            {
                          var userData = await GetUser(user.UserName, user.Password);
                          var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

                          if (user != null)
                          {
                            var claims = new[]
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                new Claim("Id",user.UserId.ToString()),
                                new Claim("UserName", user.UserName),
                                new Claim("UserName", user.Password)

                            };
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                jwt.Issuer,
                                jwt.Audience,
                                claims,
                                expires: DateTime.Now.AddMinutes(20),
                                signingCredentials: signIn
                            );

                            return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                          }
                          else
            {
                return BadRequest("Invalid Credentials");
            }
            }
            else
            {
                return BadRequest("Invalid Credentials");
            }
        }

        [HttpGet]

        public async Task<User> GetUser(string username, string password)
        {
            return await _context.user.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }

    }
}