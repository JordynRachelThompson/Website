using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Data;
using MyWebsite.Models;

namespace MyWebsite.Controllers
{
    public class BudgetItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BudgetItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.BudgetItems.ToListAsync());
        }

        // GET: BudgetItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetItems = await _context.BudgetItems
                .SingleOrDefaultAsync(m => m.Id == id);
            if (budgetItems == null)
            {
                return NotFound();
            }

            return View(budgetItems);
        }

        // GET: BudgetItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BudgetItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email")] BudgetItems budgetItems)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budgetItems);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(budgetItems);
        }

        // GET: BudgetItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetItems = await _context.BudgetItems.SingleOrDefaultAsync(m => m.Id == id);
            if (budgetItems == null)
            {
                return NotFound();
            }
            return View(budgetItems);
        }

        // POST: BudgetItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email")] BudgetItems budgetItems)
        {
            if (id != budgetItems.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetItemsExists(budgetItems.Id))
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
            return View(budgetItems);
        }

        // GET: BudgetItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetItems = await _context.BudgetItems
                .SingleOrDefaultAsync(m => m.Id == id);
            if (budgetItems == null)
            {
                return NotFound();
            }

            return View(budgetItems);
        }

        // POST: BudgetItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budgetItems = await _context.BudgetItems.SingleOrDefaultAsync(m => m.Id == id);
            _context.BudgetItems.Remove(budgetItems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetItemsExists(int id)
        {
            return _context.BudgetItems.Any(e => e.Id == id);
        }
    }
}
