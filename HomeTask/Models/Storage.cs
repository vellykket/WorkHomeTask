using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace HomeTask.Models
{
    public class Storage
    {
        public int StorageId { get; set; }
        public string StorageName { get; set; }
        public string Address { get; set; }
        
        public List<Product> Products { get; set; } = new List<Product>();
        
        public List<ProductsInStock> ProductsInStocks { get; set; } = new List<ProductsInStock>();

    }
}