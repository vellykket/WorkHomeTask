using Microsoft.AspNetCore.Mvc;

namespace HomeTask.Models
{
    public class ProductsInStock
    {
        [Remote("VerifyProductInStock", "Validator", AdditionalFields = nameof(ProductId), ErrorMessage = "Product already exist in this storage")]
        public int StorageId { get; set; } 
        public Storage Storage { get; set; }
        [Remote("VerifyProductInStock", "Validator", AdditionalFields = nameof(StorageId), ErrorMessage = "Product already exist in this storage")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}