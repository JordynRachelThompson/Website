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
    public class BudgetTransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetTransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BudgetEntries
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(BudgetTransactions budget)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budget);
                _context.SaveChanges();
                return RedirectToAction("Index", "BudgetTransactions");
            }
            return RedirectToAction("Index", "BudgetTransactions");
        }

        
        // GET: BudgetTransactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetTransactions = await _context.BudgetTransactions
                .SingleOrDefaultAsync(m => m.TransactionId == id);
            if (budgetTransactions == null)
            {
                return NotFound();
            }

            return View(budgetTransactions);
        }

        // GET: BudgetTransactions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BudgetTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,TypeOfBudget,Description,Cost,BudgetRefId")] BudgetTransactions budgetTransactions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budgetTransactions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(budgetTransactions);
        }

        // GET: BudgetTransactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetTransactions = await _context.BudgetTransactions.SingleOrDefaultAsync(m => m.TransactionId == id);
            if (budgetTransactions == null)
            {
                return NotFound();
            }
            return View(budgetTransactions);
        }

        // POST: BudgetTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,TypeOfBudget,Description,Cost,BudgetRefId")] BudgetTransactions budgetTransactions)
        {
            if (id != budgetTransactions.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetTransactions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetTransactionsExists(budgetTransactions.TransactionId))
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
            return View(budgetTransactions);
        }

        // GET: BudgetTransactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetTransactions = await _context.BudgetTransactions
                .SingleOrDefaultAsync(m => m.TransactionId == id);
            if (budgetTransactions == null)
            {
                return NotFound();
            }

            return View(budgetTransactions);
        }

        // POST: BudgetTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budgetTransactions = await _context.BudgetTransactions.SingleOrDefaultAsync(m => m.TransactionId == id);
            _context.BudgetTransactions.Remove(budgetTransactions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetTransactionsExists(int id)
        {
            return _context.BudgetTransactions.Any(e => e.TransactionId == id);
        }
    }
}
