using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace HomeTask.ViewModels
{
    public class CategoryView
    {
        [DisplayName("Product Category")]
        [Remote("VerifyProductCategoriesName", "Validator", ErrorMessage = "Category already exist")]
        public string ProductCategoryName { get; set; }
    }
}