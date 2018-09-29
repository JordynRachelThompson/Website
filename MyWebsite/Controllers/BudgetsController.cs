using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Data;
using MyWebsite.Models.BudgetProject;


namespace MyWebsite.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index(string userName)
        {
            float totalGrocery = 0;
            float totalHousing = 0;
            float totalBills = 0;
            float totalEntertainment = 0;
            float totalGas = 0;
            float totalMisc = 0;

            if (userName == null)
            {
                TempData["isBudgetEmpty"] = true;
                return View();
            }
            else
            {
                var budget = _context.Budget.Where(x => x.Email == userName);
                int budgetId = budget.Select(x => x.Id).FirstOrDefault();

                if(budgetId == 0)
                {
                    TempData["isBudgetEmpty"] = true; 
                    return View();
                }
                else
                {
                    TempData["isBudgetEmpty"] = false;

                    foreach(var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => Convert.ToInt32(x.TypeOfBudget) == 1))
                    {
                        float cost = item.Cost;
                        totalGrocery += cost;
                         
                    }

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => Convert.ToInt32(x.TypeOfBudget) == 2))
                    {
                        float cost = item.Cost;
                        totalHousing += cost;

                    }

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => Convert.ToInt32(x.TypeOfBudget) == 3))
                    {
                        float cost = item.Cost;
                        totalBills += cost;

                    }

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => Convert.ToInt32(x.TypeOfBudget) == 4))
                    {
                        float cost = item.Cost;
                        totalEntertainment += cost;

                    }

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => Convert.ToInt32(x.TypeOfBudget) == 5))
                    {
                        float cost = item.Cost;
                        totalGas += cost;

                    }

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => Convert.ToInt32(x.TypeOfBudget) == 6))
                    {
                        float cost = item.Cost;
                        totalMisc += cost;

                    }
                                                       
                    TempData["groceryTotal"] = totalGrocery;
                    TempData["housingTotal"] = totalHousing;
                    TempData["billsTotal"] = totalBills;
                    TempData["entTotal"] = totalEntertainment;
                    TempData["gasTotal"] = totalGas;
                    TempData["miscTotal"] = totalMisc;

                    return View(budget);
                }
            }         
        }


        //Set Budget Limits
        [HttpPost]
        public ActionResult Index(Budget budget)
        {
            if (ModelState.IsValid)
            {                
                _context.Add(budget);
                _context.SaveChanges();
                return View(budget);
            }
            return View(budget);           
        }

        //Add Transaction
        public ActionResult AddTransaction()
        {
            string username = User.Identity.Name;
            var budget = _context.BudgetItems.Where(x => x.Email == username).ToList();
            return View(budget);
        }

        [HttpPost]
        public ActionResult AddTransaction(BudgetItems budgetItems)
        {
            bool errors = false;
            string username = User.Identity.Name;
            var budget = _context.BudgetItems.Where(x => x.Email == username).ToList();

            //Validation
            if (Convert.ToInt32(budgetItems.TypeOfBudget) == 0)
            {
                ViewBag.typeError = "Please select a budget type from the dropdown.";
                errors = true;
            }
                        
            if (budgetItems.Cost <= 0)
            {
                ViewBag.costError = "Please enter a description and price for your transaction.";
                errors = true;
            }

            if (!errors)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(budgetItems);
                    _context.SaveChanges();
                    var budgetList = _context.BudgetItems.Where(x => x.Email == username).ToList();
                    return View(budgetList);
                }
            }
            return View(budget);
        }

        // GET: Budgets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget
                .SingleOrDefaultAsync(m => m.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // GET: Budgets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,GroceryLimit,HousingLimit,EntLimit,BillsLimit,GasLimit,MiscLimit")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget.SingleOrDefaultAsync(m => m.Id == id);
            if (budget == null)
            {
                return NotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,GroceryLimit,HousingLimit,EntLimit,BillsLimit,GasLimit,MiscLimit")] Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetExists(budget.Id))
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
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget
                .SingleOrDefaultAsync(m => m.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budget = await _context.Budget.SingleOrDefaultAsync(m => m.Id == id);
            _context.Budget.Remove(budget);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetExists(int id)
        {
            return _context.Budget.Any(e => e.Id == id);
        }
    }
}
