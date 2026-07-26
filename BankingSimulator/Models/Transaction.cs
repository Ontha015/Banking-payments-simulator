using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSimulator.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty; // "Deposit", "Withdraw", "Transfer"
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Completed"; // "Completed", "Flagged", "Blocked"

        public int AccountId { get; set; }
        public Account? Account { get; set; }

       
        

    }
}
