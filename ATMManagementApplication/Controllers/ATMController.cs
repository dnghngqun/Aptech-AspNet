using Microsoft.AspNetCore.Mvc;
using ATMManagementApplication.Models;
using ATMManagementApplication.Data;
using System.Linq;
using System;


namespace ATMManagementApplication.Controllers{
    [ApiController]
    [Route("api/ATM")]
    public class ATMController : ControllerBase{

        private readonly ATMContext _context;    
        public ATMController( ATMContext context ){
            _context = context;}

            [HttpGet("balance/{cusID}")]
            public IActionResult GetBalance(int cusID){
                var customer = _context.Customers.Find(cusID);
                if(customer != null){
                    return Ok(new {balance = customer.Balance});
                }
                return NotFound("Customer not found!");
            }

            [HttpGet("transactions/{cusID}")]
            public IActionResult GetTransactions(int cusID){
                var transactions = _context.Transactions.Where(t => t.CustomerId == cusID).ToList();
                if (transactions == null || transactions.Count == 0){
                    return NotFound("No transactions found for this customer.");
                }
                else return Ok(transactions);
            }


            
            //TODO: tranfer and notify by email
            



            [HttpPost("deposit")]
            public IActionResult Deposit([FromBody] Request request){
                var customer = _context.Customers.Find(request.CustomerId);
                if(customer == null){
                    return NotFound("");
                }
                try{
                    customer.Balance += request.Amount;
                    var transaction = new Transaction
                    {
                        CustomerId = request.CustomerId,
                        Amount = request.Amount,
                        Timestamp = DateTime.Now,
                        IsSuccessful = true,
                        TransactionType= TransactionType.Deposit
                    };

                    _context.Transactions.Add(transaction);// add to dbset

                    _context.SaveChanges(); //save dbset to database

                    return Ok(new {message = "Deposit successful", newBalance = customer.Balance});
                }catch(Exception e){
                    Console.WriteLine("Error: " + e);
                    return Problem("An error occured, please try again later", statusCode: 500);
                }
            }

            [HttpPost("withdraw")]
            public IActionResult Withdraw([FromBody] Request request){
                var customer = _context.Customers.Find(request.CustomerId);
                if (customer == null) { 
                    return NotFound("Customer not found!");
                }

                if(customer.Balance < request.Amount){
                    return BadRequest("Insufficient balance!");
                }

                try{
                
                customer.Balance -= request.Amount;

                var transaction = new Transaction
                {
                    CustomerId = request.CustomerId,
                    Amount =  request.Amount,
                    Timestamp = DateTime.Now,
                    IsSuccessful = true,
                    TransactionType= TransactionType.Withdraw
                };

                _context.Transactions.Add(transaction);// add to dbset

                _context.SaveChanges(); //save dbset to database

                return Ok(new {message = "Withdrawal successful", newBalance = customer.Balance});
            }catch(Exception e){
                Console.WriteLine("Error: " + e);
                return Problem("An error occured, please try again later", statusCode: 500);
            }
            
        }

    }
    public class Request { 
        public int CustomerId { get; set; }
        public decimal Amount{ get; set; }
    }

}