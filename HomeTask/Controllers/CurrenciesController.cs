using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeTask.Controllers.UtilityControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeTask.Models;
using HomeTask.ViewModels;

namespace HomeTask.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly HomeTaskContext _context;
        
        public CurrenciesController(HomeTaskContext context)
        {
            _context = context;
        }

        // GET: Currencies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Currencies.ToListAsync());
        }

        // GET: Currencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var currency = await _context.Currencies
                .FirstOrDefaultAsync(m => m.CurrencyId == id);
            return currency == null ? (IActionResult) NotFound() : View(currency);
        }

        // GET: Currencies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Currencies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CurrencyView currencyView)
        {
            if (!ModelState.IsValid) return View(currencyView);
            var currency = new Currency
            {
                CC = currencyView.CC,
                ExchangeDate = DateTime.Today.ToString(DateTime.Today.ToString("dd.MM.yyyy")),
                r030 = currencyView.r030,
                Rate = currencyView.Rate
            };
            _context.Add(currency);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> InitialCreate(Currency currency)
        {
            if (currency == null) throw new ArgumentNullException(nameof(currency));
            var currencies = await UtilityCurrencies.TakeData();
            UtilityCurrencies.AddingUah(currencies);
            UtilityCurrencies.Recalculate(currencies);
            foreach (var apiData in currencies)
            {
                currency = apiData;
                _context.Add(currency);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateCurrencies()
        {
            if (!Enumerable.Any(_context.Currencies, currency => currency != null)) return NotFound();
            if (!Enumerable.Any(_context.Products, product => product != null)) return NotFound();
            var products = await _context.Products.ToListAsync();
            var currenciesInDatabase = await _context.Currencies.ToListAsync();
            var newData = await UtilityCurrencies.TakeData();
            UtilityCurrencies.AddingUah(newData);
            UtilityCurrencies.Recalculate(newData);
            UtilityProduct.CalculateBasicCurrencies(newData, products);
            foreach (var oldData in currenciesInDatabase)
            {
                foreach (var editedData in newData.Where(editedData => oldData.CC == editedData.CC))
                {
                    oldData.Rate = editedData.Rate;
                    oldData.ExchangeDate = DateTime.Today.ToString("dd.MM.yyyy");
                    _context.Update(oldData);
                }
            }

            foreach (var product in products)
            {
                _context.Update(product);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Currencies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var currency = await _context.Currencies.FindAsync(id);
            return currency == null ? (IActionResult) NotFound() : View(currency);
        }

        // POST: Currencies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Currency currency)
        {
            if (id != currency.CurrencyId) return NotFound();
            if (!ModelState.IsValid) return View(currency);
            try
            {
                _context.Update(currency);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return !CurrencyExists(currency.CurrencyId)
                    ? (IActionResult) NotFound()
                    : RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Currencies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var currency = await _context.Currencies
                .FirstOrDefaultAsync(m => m.CurrencyId == id);
            return currency == null ? (IActionResult) NotFound() : View(currency);
        }

        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currency = await _context.Currencies.FindAsync(id);
            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrencyExists(int id)
        {
            return _context.Currencies.Any(e => e.CurrencyId == id);
        }
    }
}
