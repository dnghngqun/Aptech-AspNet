using Microsoft.AspNetCore.Mvc;
using ATMManagementApplication.Models;
using ATMManagementApplication.Data;

namespace ATMManagementApplication.Controllers{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase{
        private readonly ATMContext _context;
        public AuthController(ATMContext context) {
            _context = context;
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
                return Ok(customer);
            }
            return Unauthorized("Invalid Email or password");
        }

        //change pass
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