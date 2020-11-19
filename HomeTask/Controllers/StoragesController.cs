using System.Linq;
using System.Threading.Tasks;
using HomeTask.Controllers.UtilityControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeTask.Models;
using HomeTask.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeTask.Controllers
{
    public class StoragesController : Controller
    {
        private readonly HomeTaskContext _context;

        public StoragesController(HomeTaskContext context)
        {
            _context = context;
        }

        // GET: Storages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Storages.ToListAsync());
        }

        // GET: Storages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var storage = await _context.Storages
                .FirstOrDefaultAsync(m => m.StorageId == id);
            return storage == null ? (IActionResult) NotFound() : View(storage);
        }

        // GET: Storages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Storages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StorageView storageView)
        {
            if (!ModelState.IsValid) return View(storageView);
            var storage = UtilityStorage.Create(storageView);
            _context.Add(storage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Storages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var storage = await _context.Storages.FindAsync(id);
            return storage == null ? (IActionResult) NotFound() : View(storage);
        }

        // POST: Storages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Storage storage)
        {
            if (id != storage.StorageId) return NotFound();
            if (!ModelState.IsValid) return View(storage);
            try
            {
                _context.Update(storage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return !StorageExists(storage.StorageId) ? (IActionResult) NotFound() : RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Storages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var storage = await _context.Storages
                .FirstOrDefaultAsync(m => m.StorageId == id);
            return storage == null ? (IActionResult) NotFound() : View(storage);
        }

        // POST: Storages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storage = await _context.Storages.FindAsync(id);
            _context.Storages.Remove(storage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult CalculateSum(int? id)
        {
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "CC");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculateSum(int? id, CalculateSumViewModel viewModel)
        {
            if (id == null) return NotFound();
            
            var productsInStock = await _context.ProductsInStock
                .Include(stock => stock.Product )
                .ToListAsync();
            
            if (productsInStock == null) return NotFound();
            
            foreach (var products in productsInStock.Where(products => products.StorageId == id))
            {
                viewModel.Sum += (products.Count * products.Product.BasicPrice);
            }
            
            var currency = _context.Currencies.FirstOrDefault(curr => curr.CurrencyId == viewModel.CurrencyId);
            if (currency == null) return NotFound();
            viewModel.Sum *= currency.Rate;  
            ViewData["CurrencyId"] = new SelectList(_context.Currencies, "CurrencyId", "CC");
            return View("SumResult", viewModel);
        }
        
        private bool StorageExists(int id)
        {
            return _context.Storages.Any(e => e.StorageId == id);
        }
    }
}
