using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using work_01.Models.ViewModels;
using work_01.Models;

namespace work_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTTokenController : ControllerBase
    {
        public IConfiguration _configuration;
        CandidateDbContext _context;
        public JWTTokenController(IConfiguration configuration, CandidateDbContext context)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> PostLoginDetails(UserModel _userData)
        {
            if (_userData != null)
            {
                var resultLoginCheck = _context.Candidates
                    .Where(e => e.Email == _userData.Email && e.Password == _userData.Password)
                    .FirstOrDefault();
                if (resultLoginCheck == null)
                {
                    return BadRequest("Invalid Credentials");
                }
                else
                {


                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", _userData.ID.ToString()),
                        new Claim("DisplayName", _userData.FullName),
                        new Claim("Email", _userData.Email)
                    };


                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);


                    _userData.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(_userData);
                }
            }
            else
            {
                return BadRequest("No Data Posted");
            }
        }
    }
}
