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
        
        private bool BudgetExists(int id)
        {
            return _context.Budget.Any(e => e.Id == id);
        }

        public int GetLimit(string limitType, int budgetMonth)
        {
            if (limitType == "Grocery")
            {
                var limit = _context.Budget.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == budgetMonth).Select(x => x.GroceryLimit).FirstOrDefault();
                return Convert.ToInt32(limit);
            }

            if (limitType == "Housing")
            {
                var limit = _context.Budget.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == budgetMonth).Select(x => x.HousingLimit).FirstOrDefault();
                return Convert.ToInt32(limit);
            }

            if (limitType == "Bills")
            {
                var limit = _context.Budget.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == budgetMonth).Select(x => x.BillsLimit).FirstOrDefault();
                return Convert.ToInt32(limit);
            }

            if (limitType == "Entertainment")
            {
                var limit = _context.Budget.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == budgetMonth).Select(x => x.EntLimit).FirstOrDefault();
                return Convert.ToInt32(limit);
            }

            if (limitType == "Gas")
            {
                var limit = _context.Budget.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == budgetMonth).Select(x => x.GasLimit).FirstOrDefault();
                return Convert.ToInt32(limit);
            }

            if (limitType == "Misc")
            {
                var limit = _context.Budget.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == budgetMonth).Select(x => x.MiscLimit).FirstOrDefault();
                return Convert.ToInt32(limit);
            }
            else
            {
                return 0;
            }
        }

        public int GetTotalLimitByMonth(int month)
        {
            int grocery = GetLimit("Grocery", month);
            int housing = GetLimit("Housing", month);
            int bills = GetLimit("Bills", month);
            int ent = GetLimit("Ent", month);
            int gas = GetLimit("Gas", month);
            int misc = GetLimit("Misc", month);

            int sum = grocery + housing + bills + ent + gas + misc;
            return sum;
        }

        public int TotalSpentByMonth(int month)
        {
            var budgetItemsTotal = _context.BudgetItems.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == month).Select(x => x.Cost).Sum();

            return Convert.ToInt32(budgetItemsTotal); 
        }


    public BudgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Index
        public ActionResult Index(bool success)
        {
            string userName = User.Identity.Name;
            int month = DateTime.Now.Month;

            if (success)
            {
                TempData["successLimitsUpdated"] = "Budget Limits successfully updated!";
            }

            int totalBudget = 0;
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
                var budget = _context.Budget.Where(x => x.Email == userName).Where(x => x.Month == month);
                int budgetId = budget.Select(x => x.Id).FirstOrDefault();

                if (budgetId == 0)
                {
                    TempData["isBudgetEmpty"] = true;
                    var pastBudget = _context.Budget.Where(x => x.Email == User.Identity.Name);
                    if (pastBudget != null)
                    {
                        TempData["hasPastBudget"] = true;
                    }
                    else
                    {
                        TempData["hasPastBudget"] = false;
                    }
                    return View();
                }
                else
                {
                    TempData["isBudgetEmpty"] = false;

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => x.Month == month).Where(x => Convert.ToInt32(x.TypeOfBudget) == 1))
                    {
                        float cost = item.Cost;
                        totalGrocery += cost;

                    }

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => x.Month == month).Where(x => Convert.ToInt32(x.TypeOfBudget) == 2))
                    {
                        float cost = item.Cost;
                        totalHousing += cost;

                    }

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => x.Month == month).Where(x => Convert.ToInt32(x.TypeOfBudget) == 3))
                    {
                        float cost = item.Cost;
                        totalBills += cost;

                    }

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => x.Month == month).Where(x => Convert.ToInt32(x.TypeOfBudget) == 4))
                    {
                        float cost = item.Cost;
                        totalEntertainment += cost;

                    }

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => x.Month == month).Where(x => Convert.ToInt32(x.TypeOfBudget) == 5))
                    {
                        float cost = item.Cost;
                        totalGas += cost;

                    }

                    foreach (var item in _context.BudgetItems.Where(x => x.Email == userName).Where(x => x.Month == month).Where(x => Convert.ToInt32(x.TypeOfBudget) == 6))
                    {
                        float cost = item.Cost;
                        totalMisc += cost;
                    }

                    int groceryLimit = GetLimit("Grocery", month);
                    int housingLimit = GetLimit("Housing", month);
                    int billsLimit = GetLimit("Bills", month);
                    int entLimit = GetLimit("Entertainment", month);
                    int gasLimit = GetLimit("Gas", month);
                    int miscLimit = GetLimit("Misc", month);
                    totalBudget = (groceryLimit + housingLimit + billsLimit + entLimit + gasLimit + miscLimit);

                    float totalSpent = totalGrocery + totalHousing + totalBills + totalEntertainment + totalGas + totalMisc;

                    TempData["totalSpent"] = totalSpent;
                    TempData["TotalLimit"] = totalBudget;
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
        public ActionResult Index(Budget budget, bool usePastBudgetLimit)
        {
            if (ModelState.IsValid)
            {
                if (usePastBudgetLimit)
                {
                    var pastBudget = _context.Budget.Where(x => x.Email == User.Identity.Name).Select(x => x).FirstOrDefault();
                    budget.GroceryLimit = pastBudget.GroceryLimit;
                    budget.HousingLimit = pastBudget.HousingLimit;
                    budget.BillsLimit = pastBudget.BillsLimit;
                    budget.EntLimit = pastBudget.EntLimit;
                    budget.GasLimit = pastBudget.GasLimit;
                    budget.MiscLimit = pastBudget.MiscLimit;
                }
                _context.Add(budget);
                _context.SaveChanges();
                return RedirectToAction("Index", new { userName = User.Identity.Name });
            }
            else
            {
                return View();
            }

        }

        //Add Transaction
        public ActionResult AddTransaction()
        {
            string username = User.Identity.Name;
            int budgetMonth = DateTime.Now.Month;
            var budget = _context.BudgetItems.Where(x => x.Email == username).Where(x => x.Month == budgetMonth).ToList();
            return View(budget);
        }

        [HttpPost]
        public ActionResult AddTransaction(BudgetItems budgetItems)
        {
            bool errors = false;
            string username = User.Identity.Name;
            int budgetMonth = DateTime.Now.Month;

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
                    TempData["successTransactionAdded"] = ($"Transaction Added! {budgetItems.Description}: ${budgetItems.Cost}");
                    var budgetList = _context.BudgetItems.Where(x => x.Email == username).Where(x => x.Month == budgetMonth).ToList();
                    return View(budgetList);
                }
            }
            var budget = _context.BudgetItems.Where(x => x.Email == username).Where(x => x.Month == budgetMonth).ToList();
            return View(budget);
        }

        public ActionResult PastBudgets(bool deleted, string description)
        {
            TempData["jan"] = false;
            TempData["feb"] = false;
            TempData["march"] = false;
            TempData["apr"] = false;
            TempData["may"] = false;
            TempData["june"] = false;
            TempData["july"] = false;
            TempData["aug"] = false;
            TempData["sept"] = false;
            TempData["oct"] = false;
            TempData["nov"] = false;
            TempData["dec"] = false;


            //Budget totals for pie charts

            TempData["janTotal"] = GetTotalLimitByMonth(1);
            TempData["febTotal"] = GetTotalLimitByMonth(2);
            TempData["marchTotal"] = GetTotalLimitByMonth(3);
            TempData["aprTotal"] = GetTotalLimitByMonth(4);
            TempData["mayTotal"] = GetTotalLimitByMonth(5);
            TempData["juneTotal"] = GetTotalLimitByMonth(6);
            TempData["julyTotal"] = GetTotalLimitByMonth(7);
            TempData["augTotal"] = GetTotalLimitByMonth(8);
            TempData["septTotal"] = GetTotalLimitByMonth(9);
            TempData["octTotal"] = GetTotalLimitByMonth(10);
            TempData["novTotal"] = GetTotalLimitByMonth(11);
            TempData["decTotal"] = GetTotalLimitByMonth(12);

            TempData["spentInJan"] = TotalSpentByMonth(1);
            TempData["spentInFeb"] = TotalSpentByMonth(2);
            TempData["spentInMarch"] = TotalSpentByMonth(3);
            TempData["spentInApr"] = TotalSpentByMonth(4);
            TempData["spentInMay"] = TotalSpentByMonth(5);
            TempData["spentInJune"] = TotalSpentByMonth(6);
            TempData["spentInJuly"] = TotalSpentByMonth(7);
            TempData["spentInAug"] = TotalSpentByMonth(8);
            TempData["spentInSept"] = TotalSpentByMonth(9);
            TempData["spentInOct"] = TotalSpentByMonth(10);
            TempData["spentInNov"] = TotalSpentByMonth(11);
            TempData["spentInDec"] = TotalSpentByMonth(12);


            if (deleted)
            {
                description = description.ToUpper();
                ViewBag.Deleted = ($"Transaction titled {description} was successfully deleted!");
            }

            var pastBudgetTransactions = _context.BudgetItems.Where(x => x.Email == User.Identity.Name).ToList();

            //Setting month to true if budget for that month exists
            foreach (var transaction in pastBudgetTransactions)
            {
                switch (transaction.Month)
                {
                    case 1:
                        TempData["jan"] = true;
                        break;

                    case 2:
                        TempData["feb"] = true;
                        break;

                    case 3:
                        TempData["march"] = true;
                        break;

                    case 4:
                        TempData["apr"] = true;
                        break;

                    case 5:
                        TempData["may"] = true;
                        break;

                    case 6:
                        TempData["june"] = true;
                        break;

                    case 7:
                        TempData["july"] = true;
                        break;

                    case 8:
                        TempData["aug"] = true;
                        break;

                    case 9:
                        TempData["sept"] = true;
                        break;

                    case 10:
                        TempData["oct"] = true;
                        break;

                    case 11:
                        TempData["nov"] = true;
                        break;

                    case 12:
                        TempData["dec"] = true;
                        break;

                }
            }

            return View(pastBudgetTransactions);
        }

        // GET: Budgets/Details/
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
            TempData["userId"] = id;
            return View(budget);
        }

        // POST: Budgets/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,GroceryLimit,HousingLimit,EntLimit,BillsLimit,GasLimit,MiscLimit, Month")] Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }
            
                try
                {
                    _context.Update(budget);
                    await _context.SaveChangesAsync();
                    bool editSucess = true;
                    string username = User.Identity.Name;
                    return RedirectToAction("Index", new { userName = username, success = editSucess });
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
        }

        //Delete Specific Transaction
        public IActionResult DeleteTransaction(int? id)
        {
            var budgetTransaction = _context.BudgetItems.SingleOrDefault(x => x.TransactionId == id);
            if (budgetTransaction != null)
            {
                _context.BudgetItems.Remove(budgetTransaction);
                _context.SaveChanges();               
            }
            return RedirectToAction("AddTransaction");
        }

        //Delete Past Specific Transaction
        public IActionResult DeletePastTransaction(int? id)
        {
            var budgetTransaction = _context.BudgetItems.SingleOrDefault(x => x.TransactionId == id);
            if (budgetTransaction != null)
            {
                _context.BudgetItems.Remove(budgetTransaction);
                _context.SaveChanges();

                bool itemDeleted = true;
                string itemDeletedDescr = budgetTransaction.Description;
                return RedirectToAction("PastBudgets", new { deleted = itemDeleted, description = itemDeletedDescr });

            }

            return RedirectToAction("PastBudgets");

        }

        // POST: Budgets/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budget = await _context.Budget.SingleOrDefaultAsync(m => m.Id == id);
            _context.Budget.Remove(budget);
            await _context.SaveChangesAsync();
            return RedirectToAction("AddTransaction");
        }

    }
}
