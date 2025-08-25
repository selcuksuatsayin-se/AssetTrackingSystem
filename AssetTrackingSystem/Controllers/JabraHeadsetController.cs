using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetTrackingSystem.Data;
using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Controllers
{
    public class JabraHeadsetController : Controller
    {
        private readonly AssetTrackingDbContext _context;

        public JabraHeadsetController(AssetTrackingDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.JabraHeadsets.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jabraHeadset = await _context.JabraHeadsets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jabraHeadset == null)
            {
                return NotFound();
            }

            return View(jabraHeadset);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserSurname,Model,SerialNumber,Note,PurchaseDate,InvoiceStartDate,InvoiceEndDate,LastUsedDate,IsInWarehouse,IsFaulty,FaultNote,CreatedDate")] JabraHeadset jabraHeadset)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jabraHeadset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jabraHeadset);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jabraHeadset = await _context.JabraHeadsets.FindAsync(id);
            if (jabraHeadset == null)
            {
                return NotFound();
            }
            return View(jabraHeadset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,UserSurname,Model,SerialNumber,Note,PurchaseDate,InvoiceStartDate,InvoiceEndDate,LastUsedDate,IsInWarehouse,IsFaulty,FaultNote,CreatedDate")] JabraHeadset jabraHeadset)
        {
            if (id != jabraHeadset.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jabraHeadset);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JabraHeadsetExists(jabraHeadset.Id))
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
            return View(jabraHeadset);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jabraHeadset = await _context.JabraHeadsets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jabraHeadset == null)
            {
                return NotFound();
            }

            return View(jabraHeadset);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jabraHeadset = await _context.JabraHeadsets.FindAsync(id);
            if (jabraHeadset != null)
            {
                _context.JabraHeadsets.Remove(jabraHeadset);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JabraHeadsetExists(int id)
        {
            return _context.JabraHeadsets.Any(e => e.Id == id);
        }
    }
}
