using ATMApp.Data;
using Microsoft.AspNetCore.Mvc;
using ATMApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ATMApp.Controllers{
    [ApiController]
    [Authorize]
    [Route("api/transactions")]
    public class TransactionController: ControllerBase{
        private readonly ATMContext _context;
        public TransactionController(ATMContext context){
            _context = context;
        }

          [HttpPost("desposit")]
        public async Task<IActionResult> Desposit(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if(account == null) return NotFound("Account not found.");

            account.Balance += amount;
            
            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                TimeStamp = DateTime.Now,
                IsSuccessful = true,
                Description = "Deposit"
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Ok(account);
        }

        [HttpPost("{id}/deposit")]
        public async Task<IActionResult> Deposits(int id , [FromBody] decimal amount)
        {
            var account = await _context.Accounts.FindAsync(id);
            if(account == null) return NotFound("Account not found.");

            account.Balance += amount;
            
            var transaction = new Transaction
            {
                AccountId = id,
                Amount = amount,
                TimeStamp = DateTime.Now,
                IsSuccessful = true,
                Description = "Deposit"
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Ok(new {account.Balance});
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(int accountId, decimal amount){
            var account = await _context.Accounts.FindAsync(accountId);
            if(account == null) return NotFound("Account not found.");

            if(account.Balance < amount) return BadRequest("Not enough balance.");

            account.Balance -= amount;

            var transaction = new Transaction{
                AccountId = accountId,
                Amount = amount,
                TimeStamp = DateTime.Now,
                IsSuccessful = true,
                Description = "Withdraw"
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Ok(account);
        
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Ok(transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Transaction updatedTransaction)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return NotFound("Transaction not found.");

            transaction.Amount = updatedTransaction.Amount;
            transaction.Description = updatedTransaction.Description;
            transaction.IsSuccessful = updatedTransaction.IsSuccessful;
            transaction.TimeStamp = updatedTransaction.TimeStamp;

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return Ok(transaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return NotFound("Transaction not found.");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return Ok("Transaction deleted.");
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            var fromAccount = await _context.Accounts.FindAsync(fromAccountId);
            var toAccount = await _context.Accounts.FindAsync(toAccountId);

            if (fromAccount == null || toAccount == null) return NotFound("One or both accounts not found.");
            if (fromAccount.Balance < amount) return BadRequest("Not enough balance in the source account.");

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            var transaction = new Transaction
            {
            AccountId = fromAccountId,
            Amount = amount,
            TimeStamp = DateTime.Now,
            IsSuccessful = true,
            Description = "Transfer to account " + toAccountId
            };

            _context.Transactions.Add(transaction);

            var transactionTo = new Transaction
            {
            AccountId = toAccountId,
            Amount = amount,
            TimeStamp = DateTime.Now,
            IsSuccessful = true,
            Description = "Transfer from account " + fromAccountId
            };

            _context.Transactions.Add(transactionTo);

            await _context.SaveChangesAsync();
            return Ok(new { FromAccountBalance = fromAccount.Balance, ToAccountBalance = toAccount.Balance });
        }

    }

}
