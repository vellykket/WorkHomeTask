using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace HomeTask.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        public string ProductName { get; set; }
        
        public decimal Price { get; set; }
        public decimal BasicPrice { get; set; }

        [DisplayName("Product Category")]
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        
        [DisplayName("Currency")]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public int BarcodeNumber { get; set; }
        
        public List<Storage> Storages { get; set; } = new List<Storage>();
        public List<ProductsInStock> ProductsInStocks { get; set; } = new List<ProductsInStock>();
    }
}