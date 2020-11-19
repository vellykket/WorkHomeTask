using System.Linq;
using HomeTask.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeTask.Controllers
{
    public class ValidatorController : Controller
    {
        private readonly HomeTaskContext _context;

        public ValidatorController(HomeTaskContext context)
        {
            _context = context;
        }
        
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyProductName(string productName)
        {
            return Json(!_context.Products.Any(product => product.ProductName == productName));
        }
        
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyProductCategoriesName(string productCategoryName)
        {
            return Json(!_context.ProductCategories.Any(category =>  category.ProductCategoryName == productCategoryName));
        }
        
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyStorageName(string storageName)
        {
            return Json(!_context.Storages.Any(storage => storage.StorageName == storageName));
        }
        
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyProductInStock(int storageId, int productId)
        {
            return Json(!_context.ProductsInStock.Any(stock => stock.ProductId == productId) || 
                        !_context.ProductsInStock.Any(stock => stock.StorageId == storageId));
        }
    }
}