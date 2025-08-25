using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetTrackingSystem.Data;
using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Controllers
{
    public class DisplayMonitorController : Controller
    {
        private readonly AssetTrackingDbContext _context;

        public DisplayMonitorController(AssetTrackingDbContext context)
        {
            _context = context;
        }

        // GET: DisplayMonitor
        public async Task<IActionResult> Index()
        {
            return View(await _context.DisplayMonitors.ToListAsync());
        }

        // GET: DisplayMonitor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var displayMonitor = await _context.DisplayMonitors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (displayMonitor == null)
            {
                return NotFound();
            }

            return View(displayMonitor);
        }

        // GET: DisplayMonitor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DisplayMonitor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserSurname,Model,SerialNumber,Note,PurchaseDate,InvoiceStartDate,InvoiceEndDate,LastUsedDate,IsInWarehouse,IsFaulty,FaultNote,CreatedDate")] DisplayMonitor displayMonitor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(displayMonitor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(displayMonitor);
        }

        // GET: DisplayMonitor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var displayMonitor = await _context.DisplayMonitors.FindAsync(id);
            if (displayMonitor == null)
            {
                return NotFound();
            }
            return View(displayMonitor);
        }

        // POST: DisplayMonitor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,UserSurname,Model,SerialNumber,Note,PurchaseDate,InvoiceStartDate,InvoiceEndDate,LastUsedDate,IsInWarehouse,IsFaulty,FaultNote,CreatedDate")] DisplayMonitor displayMonitor)
        {
            if (id != displayMonitor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(displayMonitor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisplayMonitorExists(displayMonitor.Id))
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
            return View(displayMonitor);
        }

        // GET: DisplayMonitor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var displayMonitor = await _context.DisplayMonitors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (displayMonitor == null)
            {
                return NotFound();
            }

            return View(displayMonitor);
        }

        // POST: DisplayMonitor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var displayMonitor = await _context.DisplayMonitors.FindAsync(id);
            if (displayMonitor != null)
            {
                _context.DisplayMonitors.Remove(displayMonitor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisplayMonitorExists(int id)
        {
            return _context.DisplayMonitors.Any(e => e.Id == id);
        }
    }
}
