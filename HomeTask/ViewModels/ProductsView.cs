using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HomeTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeTask.ViewModels
{
    public class ProductsView
    {
        [Remote("VerifyProductName", "Validator", ErrorMessage = "Product already exist")]
        public string ProductName { get; set; }
        
        public decimal Price { get; set; }
        public decimal BasicPrice { get; set; }
        [DisplayName("ProductCategoryName")]
        public int ProductCategoryId { get; set; }
        [DisplayName("CurrencyName")]
        [Required]
        public int CurrencyId { get; set; }
        public Currency Currency { get; } = new Currency();
        
        public int BarcodeNumber { get; set; }
    }
}