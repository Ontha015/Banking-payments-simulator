namespace BankingSimulator.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountType { get; set; } = string.Empty; // "Savings" or "Cheque"
        public decimal Balance { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User? User { get; set; }




    }
}
