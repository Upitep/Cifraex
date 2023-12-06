using System.ComponentModel.DataAnnotations;

namespace Cifraex.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
