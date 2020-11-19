using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using HomeTask.Models;

namespace HomeTask.Controllers.UtilityControllers
{
    public static class UtilityCurrencies
    {
        private const  string Url = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";
        private const string BasicCurrency = "USD";
        
        private static readonly List<string> CurrencyNames = new List<string>
        {
            "UAH",
            "EUR",
            "USD"
        };
        
        public static async Task<List<Currency>> TakeData()
        {
            var allCurrencies = await Url.GetJsonAsync<List<Currency>>();
            var currencies = allCurrencies.Where(apiData => CurrencyNames.Contains(apiData.CC)).ToList();
            return currencies;
        }

        public static void AddingUah(List<Currency> currencies)
        {
            currencies.Add(new Currency
            {
                CC = "UAH",
                r030 = 1,
                Rate = currencies.First(apiData => apiData.CC == BasicCurrency).Rate,
                ExchangeDate = DateTime.Today.ToString("dd.MM.yyyy")
            });
        }
        
        public static void Recalculate(List<Currency> currencies)
        {
            var basicCurrencyRate = currencies.First(apiData => apiData.CC == BasicCurrency).Rate;
            foreach (var apiData in currencies.Where(apiData => apiData.CC != "UAH" && apiData.CC != BasicCurrency))
            {
                
                    apiData.Rate = basicCurrencyRate / apiData.Rate;
                    apiData.Rate = Math.Round(apiData.Rate, 3);
                    apiData.ExchangeDate = DateTime.Today.ToString("dd.MM.yyyy");
            }

            var dollarCurrency = currencies.FirstOrDefault(apiData => apiData.CC == BasicCurrency);
            if (dollarCurrency == null) return;
            dollarCurrency.Rate = 1;
            dollarCurrency.ExchangeDate = DateTime.Today.ToString("dd.MM.yyyy");
        }
    }
    
}