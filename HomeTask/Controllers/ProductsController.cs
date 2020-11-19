using System.Linq;
using System.Threading.Tasks;
using HomeTask.Controllers.UtilityControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeTask.Models;
using HomeTask.ViewModels;

namespace HomeTask.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HomeTaskContext _context;

        public ProductsController(HomeTaskContext context)
        {
            _context = context;
        }
        
        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(product => product.Currency)
                .Include(product => product.ProductCategory )
                .ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products
                .Include(prod => prod.Currency )
                .Include(prod => prod.ProductCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            return product == null ? (IActionResult) NotFound() : View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductCategoryId"] = new SelectList(
                _context.ProductCategories,
                "ProductCategoryId", 
                "ProductCategoryName");
            ViewData["CurrencyId"] = new SelectList(
                _context.Currencies, 
                "CurrencyId", 
                "CC");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductsView viewProduct)
        {
            if (ModelState.IsValid)
            {
                var currency = await _context.Currencies.FindAsync(viewProduct.CurrencyId);
                viewProduct.BasicPrice = viewProduct.Price / currency.Rate;
                viewProduct.BarcodeNumber = UtilityProduct.GenerateUniqueCode();
                var product = UtilityProduct.Create(viewProduct);
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductCategoryId"] = new SelectList(
                _context.ProductCategories,
                "ProductCategoryId", 
                "ProductCategoryName");
            ViewData["CurrencyId"] = new SelectList(
                _context.Currencies, 
                "CurrencyId", 
                "CC");
            return View(viewProduct);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products.FindAsync(id);
            ViewData["ProductCategoryId"] = new SelectList(
                _context.ProductCategories,
                "ProductCategoryId", 
                "ProductCategoryName");
            ViewData["CurrencyId"] = new SelectList(
                _context.Currencies, 
                "CurrencyId", 
                "CC");
            return product == null ? (IActionResult) NotFound() : View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId) return NotFound();
            if (!ModelState.IsValid) return View(product);
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return !ProductExists(product.ProductId) ? (IActionResult) NotFound() : RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var product = await _context.Products
                .Include(prod => prod.Currency )
                .Include(prod => prod.ProductCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            return product == null ? (IActionResult) NotFound() : View(product);
        }
        
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
        
    }
}
