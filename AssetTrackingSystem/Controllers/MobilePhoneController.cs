using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetTrackingSystem.Data;
using AssetTrackingSystem.Models;

namespace AssetTrackingSystem.Controllers
{
    public class MobilePhoneController : Controller
    {
        private readonly AssetTrackingDbContext _context;

        public MobilePhoneController(AssetTrackingDbContext context)
        {
            _context = context;
        }

        // GET: MobilePhone
        public async Task<IActionResult> Index()
        {
            return View(await _context.MobilePhones.ToListAsync());
        }

        // GET: MobilePhone/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobilePhone = await _context.MobilePhones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mobilePhone == null)
            {
                return NotFound();
            }

            return View(mobilePhone);
        }

        // GET: MobilePhone/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MobilePhone/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserSurname,Model,IMEI,Memory,Note,PurchaseDate,InvoiceStartDate,InvoiceEndDate,LastUsedDate,IsInWarehouse,IsFaulty,FaultNote,CreatedDate")] MobilePhone mobilePhone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mobilePhone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mobilePhone);
        }

        // GET: MobilePhone/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobilePhone = await _context.MobilePhones.FindAsync(id);
            if (mobilePhone == null)
            {
                return NotFound();
            }
            return View(mobilePhone);
        }

        // POST: MobilePhone/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,UserSurname,Model,IMEI,Memory,Note,PurchaseDate,InvoiceStartDate,InvoiceEndDate,LastUsedDate,IsInWarehouse,IsFaulty,FaultNote,CreatedDate")] MobilePhone mobilePhone)
        {
            if (id != mobilePhone.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mobilePhone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MobilePhoneExists(mobilePhone.Id))
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
            return View(mobilePhone);
        }

        // GET: MobilePhone/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobilePhone = await _context.MobilePhones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mobilePhone == null)
            {
                return NotFound();
            }

            return View(mobilePhone);
        }

        // POST: MobilePhone/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mobilePhone = await _context.MobilePhones.FindAsync(id);
            if (mobilePhone != null)
            {
                _context.MobilePhones.Remove(mobilePhone);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MobilePhoneExists(int id)
        {
            return _context.MobilePhones.Any(e => e.Id == id);
        }
    }
}
