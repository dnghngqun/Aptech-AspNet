using Microsoft.AspNetCore.Mvc;
using ATMManagementApplication.Models;
using ATMManagementApplication.Data;
using ATMManagementApplication.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace ATMManagementApplication.Controllers{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase{
        private readonly ATMContext _context;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        public AuthController(ATMContext context, EmailService emailService, IConfiguration configuration) {
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Customer customer) {
            if (customer == null){
                return BadRequest("Customer object is null");
            }

            if (_context.Customers.Any(c => c.Email == customer.Email)) {
                return BadRequest("Email already exists");
            }

            _context.Customers.Add(customer);
                    _context.SaveChanges();
            return Ok(customer);
        }
        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login) { 
            var customer = _context.Customers
            .FirstOrDefault(c => c.Email == login.Email && c.Password == login.Password);
            if (customer != null) {
                var token = generateToken(customer);
                return Ok(new {message = "Login success!", token = token });
            }
            return Unauthorized("Invalid Email or password");
        }

        private string generateToken(Customer customer) {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKeyBytes = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
            var tokenDescription = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, customer.Name),
                    new Claim(ClaimTypes.Email, customer.Email),
                    //role

                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Audience = jwtSettings.GetSection("Audience").Get<string[]>()[0],
                Expires = DateTime.UtcNow.AddMinutes(60),
                //ký
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),SecurityAlgorithms.HmacSha256)
            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescription);
            return new JwtSecurityTokenHandler().WriteToken(token);
                // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
                // var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                // var claims = new[]{
                //     new Claim(JwtRegisteredClaimNames.Sub, customer.Email),
                //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // };
                // var audience = jwtSettings.GetSection("Audiences").Get<string[]>();

                // var token = new JwtSecurityToken(
                //     issuer:null,
                //     audience: audience.Contains("postman") ? "postman": audience[0] ,
                //     claims: claims,
                //     expires: DateTime.Now.AddMinutes(30),
                //     signingCredentials: credentials
                // );
                // var tokenHandler = new JwtSecurityTokenHandler();
                // var stringToken = tokenHandler.WriteToken(token);
                // return stringToken;
        }

        //change pass
        [Authorize]
        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePassRequest request) {
            var customer = _context.Customers
            .FirstOrDefault(c => c.Email == request.Email && c.Password == request.OldPassword);
            if (customer != null) {
                customer.Password = request.NewPassword;
                _context.SaveChanges();
                return Ok("Change password successfully");
            }
            return Unauthorized("Invalid Email or password");        
        }
    }
    public class ChangePassRequest{
        public string Email { get; set; }
        public string OldPassword{ get; set; }
        public string NewPassword{ get; set; }
    }

    public class LoginRequest{
        public string Email { get; set; }
        public string Password { get; set; }

    }
  
}