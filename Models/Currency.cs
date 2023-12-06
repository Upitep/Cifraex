using System.ComponentModel.DataAnnotations;

namespace Cifraex.Models
{
    public class Currency
    {
        [Key]
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
    }
}
