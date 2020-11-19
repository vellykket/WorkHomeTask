using System.Collections.Generic;
using System.ComponentModel;

namespace HomeTask.Models
{
    public class Currency
    {
        
        public int CurrencyId { get; set; }
        [DisplayName("CurrencyName")]
        public string CC {get; set; }
        [DisplayName("Key")]
        public int r030 { get; set; }
        [DisplayName("Course")]
        public decimal Rate { get; set; }
        [DisplayName("UpdateData")]
        public string ExchangeDate { get; set; }
        
        public List<Product> Products { get; set; } = new List<Product>();
    }
}