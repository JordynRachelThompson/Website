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
    public class BudgetEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Set()
        {
            return RedirectToAction("Index", "BudgetTransactions");
        }

        [HttpPost]
        public IActionResult Set(BudgetEntries budgetEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budgetEntry);
                _context.SaveChanges();
                return RedirectToAction("Index", "BudgetTransactions");
            }
            return RedirectToAction("Index", "Projects");
        }

        // GET: BudgetEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetEntries = await _context.BudgetEntries
                .SingleOrDefaultAsync((System.Linq.Expressions.Expression<Func<BudgetEntries, bool>>)(m => m.BudgetId == id));
            if (budgetEntries == null)
            {
                return NotFound();
            }

            return View(budgetEntries);
        }

        // GET: BudgetEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BudgetEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Email,GroceryLimit,RentLimit,EntertainmentLimit,BillsLimit,GasLimit,MiscLimit,TypeOfBudget,Description,Cost")] BudgetEntries budgetEntries)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(budgetEntries);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(budgetEntries);
        //}

        // GET: BudgetEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetEntries = await _context.BudgetEntries.SingleOrDefaultAsync((System.Linq.Expressions.Expression<Func<BudgetEntries, bool>>)(m => m.BudgetId == id));
            if (budgetEntries == null)
            {
                return NotFound();
            }
            return View(budgetEntries);
        }

        // POST: BudgetEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,GroceryLimit,RentLimit,EntertainmentLimit,BillsLimit,GasLimit,MiscLimit,TypeOfBudget,Description,Cost")] BudgetEntries budgetEntries)
        {
            if (id != budgetEntries.BudgetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetEntries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetEntriesExists(budgetEntries.BudgetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(budgetEntries);
            }
            return View(budgetEntries);
        }

        // GET: BudgetEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetEntries = await _context.BudgetEntries
                .SingleOrDefaultAsync((System.Linq.Expressions.Expression<Func<BudgetEntries, bool>>)(m => m.BudgetId == id));
            if (budgetEntries == null)
            {
                return NotFound();
            }

            return View(budgetEntries);
        }

        // POST: BudgetEntries/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var budgetEntries = await _context.BudgetEntries.SingleOrDefaultAsync((System.Linq.Expressions.Expression<Func<BudgetEntries, bool>>)(m => m.BudgetId == id));
        //    _context.BudgetEntries.Remove(budgetEntries);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool BudgetEntriesExists(int id)
        {
            return _context.BudgetEntries.Any((System.Linq.Expressions.Expression<Func<BudgetEntries, bool>>)(e => e.BudgetId == id));
        }
    }
}
