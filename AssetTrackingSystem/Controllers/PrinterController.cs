using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetTrackingSystem.Data;
using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Controllers
{
    public class PrinterController : Controller
    {
        private readonly AssetTrackingDbContext _context;

        public PrinterController(AssetTrackingDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Printers.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var printer = await _context.Printers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (printer == null)
            {
                return NotFound();
            }

            return View(printer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Location,HostName,Model,IP,SerialNumber,Note,PurchaseDate,InvoiceStartDate,InvoiceEndDate,LastUsedDate,IsInWarehouse,IsFaulty,FaultNote,CreatedDate")] Printer printer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(printer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(printer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var printer = await _context.Printers.FindAsync(id);
            if (printer == null)
            {
                return NotFound();
            }
            return View(printer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Location,HostName,Model,IP,SerialNumber,Note,PurchaseDate,InvoiceStartDate,InvoiceEndDate,LastUsedDate,IsInWarehouse,IsFaulty,FaultNote,CreatedDate")] Printer printer)
        {
            if (id != printer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(printer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrinterExists(printer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(printer);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var printer = await _context.Printers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (printer == null)
            {
                return NotFound();
            }

            return View(printer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var printer = await _context.Printers.FindAsync(id);
            if (printer != null)
            {
                _context.Printers.Remove(printer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrinterExists(int id)
        {
            return _context.Printers.Any(e => e.Id == id);
        }
    }
}
