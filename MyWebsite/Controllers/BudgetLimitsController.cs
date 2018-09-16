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
    public class BudgetLimitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetLimitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BudgetLimits
        public async Task<IActionResult> Index()
        {
            return View(await _context.BudgetLimit.ToListAsync());
        }

        // GET: BudgetLimits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetLimit = await _context.BudgetLimit
                .SingleOrDefaultAsync(m => m.Id == id);
            if (budgetLimit == null)
            {
                return NotFound();
            }

            return View(budgetLimit);
        }

        // GET: BudgetLimits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BudgetLimits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,GroceryLimit,RentLimit,BillsLimit,GasLimit,EntertainmentLimit,MiscLimit")] BudgetLimit budgetLimit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budgetLimit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(budgetLimit);
        }

        // GET: BudgetLimits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetLimit = await _context.BudgetLimit.SingleOrDefaultAsync(m => m.Id == id);
            if (budgetLimit == null)
            {
                return NotFound();
            }
            return View(budgetLimit);
        }

        // POST: BudgetLimits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,GroceryLimit,RentLimit,BillsLimit,GasLimit,EntertainmentLimit,MiscLimit")] BudgetLimit budgetLimit)
        {
            if (id != budgetLimit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetLimit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetLimitExists(budgetLimit.Id))
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
            return View(budgetLimit);
        }

        // GET: BudgetLimits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetLimit = await _context.BudgetLimit
                .SingleOrDefaultAsync(m => m.Id == id);
            if (budgetLimit == null)
            {
                return NotFound();
            }

            return View(budgetLimit);
        }

        // POST: BudgetLimits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budgetLimit = await _context.BudgetLimit.SingleOrDefaultAsync(m => m.Id == id);
            _context.BudgetLimit.Remove(budgetLimit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetLimitExists(int id)
        {
            return _context.BudgetLimit.Any(e => e.Id == id);
        }
    }
}
