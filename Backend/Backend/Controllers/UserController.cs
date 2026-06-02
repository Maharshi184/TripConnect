using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public UserController(AppDbContext dbContext, IConfiguration configuration) {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        private string GenerateToken(TblUser user)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier,
                  user.UserId.ToString()),

        new Claim(ClaimTypes.Email,
                  user.Email),

        new Claim(ClaimTypes.Name,
                  user.Name)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]));

            var creds =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }


        [HttpPost("/Login")]
        public ActionResult Login([FromBody] TblUser user ){

            var dbUser = _dbContext.TblUsers.FirstOrDefault(x => x.Email == user.Email);

            if (dbUser == null)
            {
                return Unauthorized();
            }

            bool valid = BCrypt.Net.BCrypt.Verify(
                user.Password,
                dbUser.Password);

            if (!valid)
            {
                return Unauthorized();
            }

            var token = GenerateToken(user);

            return Ok(new
            {
                token,
                user.UserId,
                user.Name,
                user.Email
            });
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] TblUser user)
        {
            try
            {
                var existingUser = await _dbContext.TblUsers
                    .FirstOrDefaultAsync(x =>
                        x.Email == user.Email ||
                        x.UserName == user.UserName);

                if (existingUser != null)
                {
                    return BadRequest("Email or Username already exists.");
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                await _dbContext.TblUsers.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return Ok("Signed up successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
