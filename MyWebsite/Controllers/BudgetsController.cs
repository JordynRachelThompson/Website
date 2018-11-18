using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Data;
using MyWebsite.Models.BudgetProject;
using System;
using System.Linq;
using System.Threading.Tasks;
using MyWebsite.Services;


namespace MyWebsite.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BudgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index(bool success)
        {
            int currentMonth = DateTime.Now.Month;
            var budget = _context.Budget.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == currentMonth);

            if (success)
                ViewBag.SuccessLimitsUpdated = "Budget Limits successfully updated!";
            
            if (!budget.Any() || User.Identity.Name == null)
            {
                ViewBag.EmptyBudget = true;
                ViewBag.HasPastBudget = false;
                var pastBudget = _context.Budget.Where(x => x.Email == User.Identity.Name);
                if (pastBudget.Any())
                    ViewBag.HasPastBudget = true;
            }

            if (!ViewBag.EmptyBudget)
            {
                var budgetService = new BudgetService(_context);

                var totalBudget = budgetService.GetTotalBudgetLimitByMonth(currentMonth, User.Identity.Name);
                TempData["TotalLimit"] = totalBudget;

                var totalSpent = budgetService.TotalSpentByMonth(currentMonth, User.Identity.Name); //totalGrocery + totalHousing + totalBills + totalEntertainment + totalGas + totalMisc;
                TempData["totalSpent"] = totalSpent;


                TempData["groceryTotal"] = budgetService.TotalSpentByBudgetCategory(currentMonth, User.Identity.Name, 1);
                TempData["housingTotal"] = budgetService.TotalSpentByBudgetCategory(currentMonth, User.Identity.Name, 2);
                TempData["billsTotal"] = budgetService.TotalSpentByBudgetCategory(currentMonth, User.Identity.Name, 3);
                TempData["entTotal"] = budgetService.TotalSpentByBudgetCategory(currentMonth, User.Identity.Name, 4);
                TempData["gasTotal"] = budgetService.TotalSpentByBudgetCategory(currentMonth, User.Identity.Name, 5);
                TempData["miscTotal"] = budgetService.TotalSpentByBudgetCategory(currentMonth, User.Identity.Name, 6);
            }

            return View(budget);
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
        public ActionResult AddTransaction(bool deleted, string description)
        {
            string username = User.Identity.Name;
            int budgetMonth = DateTime.Now.Month;
            var budget = _context.BudgetItems.Where(x => x.Email == username).Where(x => x.Month == budgetMonth).ToList();

            if (deleted)
            {
                description = description.ToUpper();
                ViewBag.Deleted = ($"Transaction titled {description} was successfully deleted!");
            }

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

            string itemDeletedDescr = budgetTransaction.Description;

            return RedirectToAction("AddTransaction", new { deleted = true, description = itemDeletedDescr });
        }

        //Delete Past Specific Transaction
        public IActionResult DeletePastTransaction(int? id)
        {
            var budgetTransaction = _context.BudgetItems.SingleOrDefault(x => x.TransactionId == id);
            if (budgetTransaction != null)
            {
                _context.BudgetItems.Remove(budgetTransaction);
                _context.SaveChanges();

                string itemDeletedDescr = budgetTransaction.Description;
                return RedirectToAction("PastBudgets", new { deleted = true, description = itemDeletedDescr });

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

        public IActionResult ExportExcel()
        {
            return View();
        }
        //public void ExportListFromTsv()
        //{
        //    TextWriter tw = new StreamWriter(Response.Body);

        //    var excelTsv = new ExcelUtil();
        //    var data = new[]{
        //        new{ Name="Ram", Email="ram@techbrij.com", Phone="111-222-3333" },
        //        new{ Name="Shyam", Email="shyam@techbrij.com", Phone="159-222-1596" },
        //        new{ Name="Mohan", Email="mohan@techbrij.com", Phone="456-222-4569" },
        //        new{ Name="Sohan", Email="sohan@techbrij.com", Phone="789-456-3333" },
        //        new{ Name="Karan", Email="karan@techbrij.com", Phone="111-222-1234" },
        //        new{ Name="Brij", Email="brij@techbrij.com", Phone="111-222-3333" }
        //    };

        //    Response.Clear();


        //    Response.Headers[HeaderNames.ContentDisposition] = "attachment; filename=DemoExcel.xls";

        //    Response.Headers[HeaderNames.ContentType] = "application/vnd.ms-excel";
        //    excelTsv.WriteTsv(data, tw);
        //    HttpContext.Response.Clear();
        //}

        public IActionResult BudgetAnalytics()
        {
            return View();
        }
    }

}
