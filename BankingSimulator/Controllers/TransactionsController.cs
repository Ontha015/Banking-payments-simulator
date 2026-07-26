using BankingSimulator.Data;
using BankingSimulator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSimulator.Controllers
{
   
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly BankingContext _context;

        public TransactionsController(BankingContext context)
        {
            _context = context;
        }

        // GET: api/transactions/account/2
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetByAccount(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }

        // POST: api/transactions/deposit
        [HttpPost("deposit")]
        public async Task<ActionResult<Transaction>> Deposit(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                return NotFound("Account not found.");

            if (amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            account.Balance += amount;

            var transaction = new Transaction
            {
                Type = "Deposit",
                Amount = amount,
                BalanceAfter = account.Balance,
                AccountId = accountId
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(transaction);
        }

        // POST: api/transactions/withdraw
        [HttpPost("withdraw")]
        public async Task<ActionResult<Transaction>> Withdraw(int accountId, decimal amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                return NotFound("Account not found.");

            if (amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            if (account.Balance < amount)
                return BadRequest("Insufficient funds.");

            account.Balance -= amount;

            var transaction = new Transaction
            {
                Type = "Withdraw",
                Amount = amount,
                BalanceAfter = account.Balance,
                AccountId = accountId
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(transaction);
        }

        // POST: api/transactions/transfer
        [HttpPost("transfer")]
        public async Task<ActionResult> Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            if (amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            if (fromAccountId == toAccountId)
                return BadRequest("Cannot transfer to the same account.");

            var fromAccount = await _context.Accounts.FindAsync(fromAccountId);
            var toAccount = await _context.Accounts.FindAsync(toAccountId);

            if (fromAccount == null || toAccount == null)
                return NotFound("One or both accounts not found.");

            if (fromAccount.Balance < amount)
                return BadRequest("Insufficient funds.");

            // Fraud check: transfer exceeds 80% of sender's balance
            bool isFlagged = amount > (fromAccount.Balance * 0.8m);

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            var outgoing = new Transaction
            {
                Type = "Transfer-Out",
                Amount = amount,
                BalanceAfter = fromAccount.Balance,
                AccountId = fromAccountId,
                Status = isFlagged ? "Flagged" : "Completed"
            };

            var incoming = new Transaction
            {
                Type = "Transfer-In",
                Amount = amount,
                BalanceAfter = toAccount.Balance,
                AccountId = toAccountId,
                Status = "Completed"
            };

            _context.Transactions.Add(outgoing);
            _context.Transactions.Add(incoming);
            await _context.SaveChangesAsync();

            return Ok(new { outgoing, incoming, flagged = isFlagged });
        }


        // GET: api/transactions/flagged
        [HttpGet("flagged")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetFlagged()
        {
            return await _context.Transactions
                .Where(t => t.Status == "Flagged")
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }

        // PUT: api/transactions/5/review
        [HttpPut("{id}/review")]
        public async Task<ActionResult> ReviewTransaction(int id, [FromQuery] string decision)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
                return NotFound("Transaction not found.");

            if (decision != "Approve" && decision != "Block")
                return BadRequest("Decision must be 'Approve' or 'Block'.");

            if (decision == "Approve")
            {
                transaction.Status = "Completed";
            }
            else // Block
            {
                transaction.Status = "Blocked";

                // Reverse the transaction's effect on the account balance
                var account = await _context.Accounts.FindAsync(transaction.AccountId);
                if (account != null)
                {
                    if (transaction.Type == "Deposit" || transaction.Type == "Transfer-In")
                        account.Balance -= transaction.Amount;
                    else if (transaction.Type == "Withdraw" || transaction.Type == "Transfer-Out")
                        account.Balance += transaction.Amount;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(transaction);
        }



    }
}
