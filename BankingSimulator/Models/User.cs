using System.Security.Principal;

namespace BankingSimulator.Models
{
    public class User
    {

        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // One user can have many accounts
        public List<Account> Accounts { get; set; } = new();




    }
}
