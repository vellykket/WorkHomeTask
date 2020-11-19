using System;
using System.Collections.Generic;
using System.Linq;
using HomeTask.Models;
using HomeTask.ViewModels;

namespace HomeTask.Controllers.UtilityControllers
{
    public static class UtilityProduct
    {
        public static Product Create(ProductsView viewProduct)
        {
            var product = new Product
            {
                ProductName = viewProduct.ProductName,
                Price = viewProduct.Price,            
                BasicPrice = viewProduct.BasicPrice,
                BarcodeNumber = viewProduct.BarcodeNumber,
                CurrencyId = viewProduct.CurrencyId,
                Currency = viewProduct.Currency,              
                ProductCategoryId = viewProduct.ProductCategoryId
            };
            return product;
        }

        public static int GenerateUniqueCode()
        {
            var code = "";
            var random = new Random();
            for (var i = 0; i < 8; i++)
            {
                code += random.Next(0, 10);
            }
            return int.Parse(code);
        }

        public static void RecalculateProductCurrencies(IEnumerable<Product> products, IEnumerable<Currency> currencies)
        {
            foreach (var product in from product in products from currency in currencies
                .Where(currency => product.CurrencyId == currency.CurrencyId) select product)
            {
                product.BasicPrice = product.Price / product.Currency.Rate;
            }
        }
        public static void CalculateBasicCurrencies(List<Currency> currencies, IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                foreach (var currency in currencies.Where(currency => product.Currency.CC == currency.CC))
                {
                    product.BasicPrice = product.Price / currency.Rate;
                }
            }
        }
    }
}