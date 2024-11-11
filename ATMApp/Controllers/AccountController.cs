using ATMApp.Data;
using Microsoft.AspNetCore.Mvc;
using ATMApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ATMApp.Controllers{

    [ApiController]
    [Route("api/accounts")]
    //controller gọi model
    public class AccountController : ControllerBase
    {
        private readonly ATMContext _context; //context thay thế cho model
        public AccountController(ATMContext context){
            _context = context;
        }
        //todo: create, update, delete account
        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAccountById), new { id = account.AccountId }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, Account account)
        {
            if (id != account.AccountId)
            {
            return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
            await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            if (!AccountExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
            return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccountById(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
            return NotFound();
            }

            return account;
        }
    
        // [HttpPost("login")]
        // public async Task<ActionResult<Account>> Login([FromBody] LoginModel loginModel)
        // {
        //     var account = await _context.Accounts
        //         .FirstOrDefaultAsync(a => a.Username == loginModel.Username && a.Password == loginModel.Password);

        //     if (account == null)
        //     {
        //         return Unauthorized();
        //     }


        //     return Ok(account);
        // }
    }
   
}

    