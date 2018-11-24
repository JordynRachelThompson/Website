using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Data;
using MyWebsite.Models.BudgetProject;
using System;
using System.Collections.Generic;
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
            if (success)
                ViewBag.SuccessLimitsUpdated = "Budget limits successfully updated!";

            ViewBag.EmptyBudget = false;
            var currentMonth = DateTime.Now.Month;
            var budget = _context.Budget.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == currentMonth);

            if (!budget.Any())
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

                var budgetTotals = new List<float>();
                for (var i = 1; i < 7; i++)
                {
                    budgetTotals.Add(budgetService.TotalSpentByBudgetCategory(currentMonth, User.Identity.Name, i));
                }

                ViewBag.BudgetTotals = budgetTotals;
            }

            return View(budget);
        }

        [HttpPost] //Set Budget Limits
        public ActionResult Index(Budget budget, bool usePastBudgetLimit)
        {
            var budgetService = new BudgetService(_context);

            if (ModelState.IsValid && usePastBudgetLimit)
                budgetService.SetBudgetLimitToPastLimit(budget);

            return View();
        }

        public ActionResult AddTransaction(bool deleted, string description)
        {
            if (deleted)
                ViewBag.Deleted = ($"Transaction titled {description.ToUpper()} was successfully deleted!");

            var budget = _context.BudgetItems.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == DateTime.Now.Month).ToList();

            return View(budget);
        }

        [HttpPost]
        public ActionResult AddTransaction(BudgetItems budgetItems)
        {
            var errors = false;

            if (Convert.ToInt32(budgetItems.TypeOfBudget) == 0)
            {
                ViewBag.typeError = "Please select a budget type from the dropdown.";
                errors = true;
            }

            if (budgetItems.Cost <= 0)
            {
                ViewBag.costError = "Please enter a price greater than $0.00 for your transaction.";
                errors = true;
            }

            if (!errors && ModelState.IsValid)
            {
                _context.Add(budgetItems);
                _context.SaveChanges();
                ViewBag.SuccessTransactionAdded = ($"Transaction Added! {budgetItems.Description}: ${budgetItems.Cost}");
            }

            var budget = _context.BudgetItems.Where(x => x.Email == User.Identity.Name).Where(x => x.Month == DateTime.Now.Month).ToList();

            return View(budget);
        }

        public ActionResult PastBudgets(bool deleted, string description)
        {
            //Budget totals for pie charts
            var budgetService = new BudgetService(_context);
            var monthListTotal = new List<float>();
            var monthListSpent = new List<float>();

            for (var monthNum = 1; monthNum < 13; monthNum++)
            {
                monthListTotal.Add(budgetService.GetTotalBudgetLimitByMonth(monthNum, User.Identity.Name));
                monthListSpent.Add(budgetService.TotalSpentByMonth(monthNum, User.Identity.Name));
            }

            ViewBag.MonthListTotal = monthListTotal;
            ViewBag.MonthListSpent = monthListSpent;

            if (deleted)
                ViewBag.Deleted = ($"Transaction titled {description.ToUpper()} was successfully deleted!");

            ViewBag.BudgetMonthList = budgetService.GetBudgetMonthsList(User.Identity.Name);

            return View(_context.BudgetItems.Where(x => x.Email == User.Identity.Name).ToList());
        }


        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var budget = await _context.Budget.SingleOrDefaultAsync(m => m.Id == id);

            if (budget == null)
                return NotFound();

            TempData["userId"] = id;

            return View(budget);
        }

        // POST: Budgets/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,GroceryLimit,HousingLimit,EntLimit,BillsLimit,GasLimit,MiscLimit, Month")] Budget budget)
        {
            if (id != budget.Id)
                return NotFound();

            _context.Update(budget);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { userName = User.Identity.Name, success = true });
        }

        //Delete Specific Transaction
        public IActionResult DeleteTransaction(int? id, string returnTo)
        {
            var budgetTransaction = _context.BudgetItems.SingleOrDefault(x => x.TransactionId == id);
            if (budgetTransaction != null)
            {
                _context.BudgetItems.Remove(budgetTransaction);
                _context.SaveChanges();

                return RedirectToAction(returnTo, new { deleted = true, description = budgetTransaction.Description });
            }

            return RedirectToAction(returnTo, new { deleted = false });
        }
        

        //// POST: Budgets/Delete
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var budget = await _context.Budget.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Budget.Remove(budget);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("AddTransaction");
        //}

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
