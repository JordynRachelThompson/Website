using Microsoft.AspNetCore.Mvc;
using MyWebsite.Data.Interfaces;
using MyWebsite.Models.BudgetApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebsite.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BudgetsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(bool success)
        {
            var user = User.Identity.Name;
            if (success)
                ViewBag.SuccessLimitsUpdated = "Budget limits successfully updated!";

            ViewBag.HasPastBudget = _unitOfWork.BudgetRepository.BudgetExistsByUser(user);
            ViewBag.NoCurrentBudget = !(_unitOfWork.BudgetRepository.CurrentBudgetExistsByUser(user, DateTime.Now.Month));
            if (ViewBag.NoCurrentBudget) return View();

            var currentBudget = _unitOfWork.BudgetRepository.GetCurrentBudget(user, DateTime.Now.Month);
            ViewBag.BudgetId = currentBudget.Id;

            var budgetTotals = new List<float>();
            for (var i = 1; i < 7; i++)
                budgetTotals.Add(_unitOfWork.BudgetItemsRepository.TotalSpentByBudgetCategory(DateTime.Now.Month, user, i));

            ViewBag.BudgetTotals = budgetTotals;

            return View(currentBudget);
        }

        [HttpPost] //Set Budget Limits
        public ActionResult Index(Budget budget, bool usePastBudgetLimit)
        {
            if (!ModelState.IsValid)
                ViewBag.BudgetLimitError = "Please choose a value for each budget limit or enter 0.";
            else
            {
                if (ModelState.IsValid && usePastBudgetLimit)
                    _unitOfWork.BudgetRepository.SetBudgetLimitToPastLimit(budget);
                else
                    _unitOfWork.BudgetRepository.SetNewBudgetLimits(budget);
            }

            return RedirectToAction("Index");
        }

        public ActionResult AddTransaction(bool deleted, string description)
        {
            if (deleted)
                ViewBag.Deleted = ($"Transaction titled {description.ToUpper()} was successfully deleted!");

            return View(_unitOfWork.BudgetItemsRepository.GetBudgetItemsListByMonth(User.Identity.Name, DateTime.Now.Month));
        }

        [HttpPost]
        public ActionResult AddTransaction(BudgetItems budgetItems)
        {
            var errors = false;

            if (Convert.ToInt32(budgetItems.TypeOfBudget) == 0)
            {
                ViewBag.Error = "Please select a budget category from the dropdown.";
                errors = true;
            }

            if (budgetItems.Cost <= 0)
            {
                ViewBag.Error = "Please enter a price greater than $0.00 for your transaction.";
                errors = true;
            }

            if (!errors && ModelState.IsValid)
            {
                budgetItems.Email = User.Identity.Name;
                budgetItems.Month = DateTime.Now.Month;
                budgetItems.Date = DateTime.Now;
                _unitOfWork.BudgetItemsRepository.Add(budgetItems);
                _unitOfWork.Complete();
                ViewBag.SuccessTransactionAdded = ($"Transaction Added! {budgetItems.Description}: ${budgetItems.Cost}");
            }

            return View(_unitOfWork.BudgetItemsRepository.GetBudgetItemsListByMonth(User.Identity.Name, DateTime.Now.Month));
        }

        public ActionResult PastBudgets(bool deleted, string description)
        {
            var monthListTotal = new List<float>();
            var monthListSpent = new List<float>();

            for (var monthNum = 1; monthNum < 13; monthNum++)
            {
                monthListTotal.Add(_unitOfWork.BudgetRepository.GetTotalBudgetLimitByMonth(monthNum, User.Identity.Name));
                monthListSpent.Add(_unitOfWork.BudgetItemsRepository.TotalSpentByMonth(monthNum, User.Identity.Name));
            }

            ViewBag.MonthListTotal = monthListTotal;
            ViewBag.MonthListSpent = monthListSpent;

            if (deleted)
                ViewBag.Deleted = ($"Transaction titled {description.ToUpper()} was successfully deleted!");

            ViewBag.BudgetMonthList = _unitOfWork.BudgetItemsRepository.GetBudgetMonthsList(User.Identity.Name);

            return View(_unitOfWork.BudgetItemsRepository.ReturnBudgetItemsListForUser(User.Identity.Name));
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var budget = _unitOfWork.BudgetRepository.GetBudgetById(Convert.ToInt32(id));

            if (budget == null)
                return NotFound();

            TempData["userId"] = id;

            return View(budget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Budget budget)
        {
            _unitOfWork.BudgetRepository.EditBudgetLimits(budget);
            _unitOfWork.Complete();

            return RedirectToAction("Index", new { userName = User.Identity.Name, success = true });
        }

        //Delete Specific Transaction
        public IActionResult DeleteTransaction(int? id, string returnTo)
        {
            var budgetTransaction = _unitOfWork.BudgetItemsRepository.GetTransactionById(Convert.ToInt32(id));

            if (budgetTransaction == null) return RedirectToAction(returnTo, new { deleted = false });

            _unitOfWork.BudgetItemsRepository.RemoveItem(budgetTransaction);
            _unitOfWork.Complete();

            return RedirectToAction(returnTo, new { deleted = true, description = budgetTransaction.Description });
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

        public IActionResult BudgetInsights()
        {
            ViewBag.AvgMonthlySpending = _unitOfWork.BudgetItemsRepository.AvgMonthlySpending(User.Identity.Name);
            ViewBag.AmtOverOrUnder = _unitOfWork.BudgetItemsRepository.AmtOverOrUnderBudget(User.Identity.Name);

            ViewBag.MostCommonCategory = _unitOfWork.BudgetItemsRepository.MostCommonCategory(User.Identity.Name);
            ViewBag.CategoryTimes = _unitOfWork.BudgetItemsRepository.HowManyTimesCatOccur(ViewBag.MostCommonCategory, User.Identity.Name);

            ViewBag.NoMothsUnderBudget = _unitOfWork.BudgetItemsRepository.NumMonthsUnderBudget(User.Identity.Name);
            ViewBag.TotalMonths = _unitOfWork.BudgetItemsRepository.GetBudgetMonthsList(User.Identity.Name).Count;

            ViewBag.FirstMonth =_unitOfWork.BudgetItemsRepository.TotalSpentByMonth(DateTime.Now.Month, User.Identity.Name);
            ViewBag.SecondMonth = _unitOfWork.BudgetItemsRepository.TotalSpentByMonth((DateTime.Now.Month - 1), User.Identity.Name);

            var highestSavings = _unitOfWork.BudgetItemsRepository.HighestSavings(User.Identity.Name).Single();
            ViewBag.HighestSaveCat = highestSavings.Key;
            ViewBag.HighestSaveAmt = highestSavings.Value;
            return View();
        }
    }

}
