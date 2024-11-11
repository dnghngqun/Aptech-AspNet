using ATMApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ATMApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ATMApp.Controllers{
    [ApiController]
    [Route("api/users")]
    public class UserController:ControllerBase{
        private readonly ATMContext _context;
        private readonly TokenService _tokenService;
        public UserController(ATMContext context, TokenService tokenService){
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == loginModel.Username && u.Password == loginModel.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Sinh JWT token cho người dùng
            var tokenService = new TokenService("WWWTgLPtTFN7udSRT8-?]FTX4C(J>Y_i7Mnb53{sMua!Ftb]g2Mmd(GNbVApM|3", "yourIssuer", "postman");
            var token = tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }

    
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.UserId)
            {
            return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
            await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
            return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }

    public class LoginModel{
        public string Username { get; set; }
        public string Password { get; set; }
    }
}