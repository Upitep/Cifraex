using System.ComponentModel.DataAnnotations;

namespace Cifraex.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public Currency Currency { get; set; }
        public decimal Balance { get; set; }

        public int UserId { get; set; }
    }
}
