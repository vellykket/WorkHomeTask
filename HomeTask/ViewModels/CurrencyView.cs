using System.ComponentModel;

namespace HomeTask.ViewModels
{
    public class CurrencyView
    {
        [DisplayName("CurrencyName")]
        public string CC {get; set; }
        [DisplayName("Key")]
        public int r030 { get; set; }
        [DisplayName("Course")]
        public decimal Rate { get; set; }
    }
}