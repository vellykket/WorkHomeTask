using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeTask.Models;

namespace HomeTask.Controllers
{
    public class ProductsInStocksController : Controller
    {
        private readonly HomeTaskContext _context;

        public ProductsInStocksController(HomeTaskContext context)
        {
            _context = context;
        }

        // GET: ProductsInStocks
        public async Task<IActionResult> Index()
        {
            var homeTaskContext = _context.ProductsInStock
                .Include(p => p.Product)
                .Include(p => p.Storage);
            return View(await homeTaskContext.ToListAsync());
        }
        
        public async Task<IActionResult> ShowProductsInStock(int? id)
        {
            var homeTaskContext = _context.ProductsInStock
                .Include(p => p.Product)
                .Include(p => p.Storage)
                .Where(stock => stock.StorageId == id);
            return View(await homeTaskContext.ToListAsync());
        }

        // GET: ProductsInStocks/Details/5
        public async Task<IActionResult> Details(int? productId, int? storageId)
        {
            if (productId == null && storageId == null) return NotFound();
            var productsInStock = await _context.ProductsInStock
                .Include(p => p.Product)
                .Include(p => p.Storage)
                .FirstOrDefaultAsync(m => m.StorageId == storageId);
            return productsInStock == null ? (IActionResult) NotFound() : View(productsInStock);
        }

        // GET: ProductsInStocks/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["StorageId"] = new SelectList(_context.Storages, "StorageId", "StorageName");
            return View();
        }

        // POST: ProductsInStocks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StorageId,ProductId,Count")] ProductsInStock productsInStock)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productsInStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductId"] = new SelectList(
                _context.Products,
                "ProductId",
                "ProductName",
                productsInStock.ProductId
            );

            ViewData["StorageId"] = new SelectList(
                _context.Storages,
                "StorageId",
                "StorageName",
                productsInStock.StorageId
            );

            return View(productsInStock);
        }

        // GET: ProductsInStocks/Edit/5
        public  IActionResult Edit(int? productId, int? storageId)
        {
            if (productId == null && storageId == null) return NotFound();
            
            var productsInStock = _context.ProductsInStock.FirstOrDefault(stock =>
                stock.ProductId == productId && stock.StorageId == storageId);
            
            if (productsInStock == null) return NotFound();
            
            ViewData["ProductId"] = new SelectList(
                _context.Products, 
                "ProductId", 
                "ProductName",
                productsInStock.ProductId);
            
            ViewData["StorageId"] = new SelectList(
                _context.Storages, 
                "StorageId", 
                "StorageName",
                productsInStock.StorageId);
            
            return View(productsInStock);
        }

        // POST: ProductsInStocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? productId, int? storageId, [Bind("StorageId,ProductId,Count")] ProductsInStock productsInStock)
        {
            if (storageId != productsInStock.StorageId && productId != productsInStock.ProductId) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productsInStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return !ProductsInStockExists(productsInStock.StorageId)
                        ? (IActionResult) NotFound()
                        : RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductId"] = new SelectList(
                _context.Products, 
                "ProductId", 
                "ProductName",
                productsInStock.ProductId);
            
            ViewData["StorageId"] = new SelectList(
                _context.Storages, 
                "StorageId", 
                "StorageName",
                productsInStock.StorageId);
            
            return View(productsInStock);
        }

        // GET: ProductsInStocks/Delete/5
        public async Task<IActionResult> Delete(int? productId, int? storageId)
        {
            if (productId == null && storageId == null) return NotFound();
            var productsInStock = await _context.ProductsInStock
                .Include(p => p.Product)
                .Include(p => p.Storage)
                .FirstOrDefaultAsync(m => m.StorageId == storageId);
            return productsInStock == null ? (IActionResult) NotFound() : View(productsInStock);
        }

        // POST: ProductsInStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? productId, int? storageId)
        {
            var productsInStock =  _context.ProductsInStock.
                FirstOrDefault(stock => stock.StorageId == storageId && stock.ProductId == productId);
            if (productsInStock == null) return NotFound();
            _context.ProductsInStock.Remove(productsInStock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProductsInStockExists(int id)
        {
            return _context.ProductsInStock.Any(e => e.StorageId == id);
        }
    }
}
