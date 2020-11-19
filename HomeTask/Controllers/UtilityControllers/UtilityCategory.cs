using HomeTask.Models;
using HomeTask.ViewModels;

namespace HomeTask.Controllers.UtilityControllers
{
    public class UtilityCategory
    {
        public static ProductCategory Create(CategoryView categoryView)
        {
            var category = new ProductCategory
            {
                ProductCategoryName = categoryView.ProductCategoryName
            };
            return category;
        }
    }
}