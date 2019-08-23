using Microsoft.AspNetCore.Mvc;
using PortfolioWebsite.Data.Interfaces;
using PortfolioWebsite.Models.BudgetApp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortfolioWebsite.Services;

namespace PortfolioWebsite.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BudgetsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var user = User.Identity.Name;
            ViewBag.SuccessLimitsUpdated = TempData["budgetUpdatedSuccess"];
            ViewBag.ErrorUpdatingLimits = TempData["budgetLimitError"];

            ViewBag.HasPastBudget = _unitOfWork.BudgetRepository.BudgetExistsByUser(user);
            ViewBag.NoCurrentBudget = !_unitOfWork.BudgetRepository.CurrentBudgetExistsByUser(user, DateTime.Now.Month);
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
                TempData["budgetLimitError"] = "Please type a value for each budget limit or enter 0";

            else
            {
                if (ModelState.IsValid && usePastBudgetLimit)
                    _unitOfWork.BudgetRepository.SetBudgetLimitToPastLimit(budget);
                else
                    _unitOfWork.BudgetRepository.SetNewBudgetLimits(budget);
                TempData["budgetUpdatedSuccess"] = "Budget limits successfully updated!";
            }

            return RedirectToAction("Index");
        }

        public ActionResult AddTransaction(bool deleted, string description)
        {
            if (deleted)
                ViewBag.Deleted = ($"Transaction titled {description.ToUpper()} was successfully deleted!");

            var transactions =
                _unitOfWork.BudgetItemsRepository.GetBudgetItemsListByMonth(User.Identity.Name, DateTime.Now.Month);

            return View(transactions);
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
                budgetItems.TypeOfBudget = 0;
            }

            var transactions =
                _unitOfWork.BudgetItemsRepository.GetBudgetItemsListByMonth(User.Identity.Name, DateTime.Now.Month);

            return View(transactions);
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

        public ActionResult Edit()
        {
            return View(_unitOfWork.BudgetRepository.GetCurrentBudget(User.Identity.Name, DateTime.Now.Month));
        }

        [HttpPost]
        public ActionResult Edit(Budget budget)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorUpdatingLimits = "Please type a value for each budget limit or enter 0";
                return View(_unitOfWork.BudgetRepository.GetCurrentBudget(User.Identity.Name, DateTime.Now.Month));
            }

            _unitOfWork.BudgetRepository.EditBudgetLimits(budget);
            _unitOfWork.Complete();
            TempData["budgetUpdatedSuccess"] = "Budget limits successfully updated!";

            return RedirectToAction("Index");
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

        public IActionResult BudgetInsights()
        {
            var user = User.Identity.Name;
            var highestSavings = _unitOfWork.BudgetItemsRepository.HighestSavings(user).Single();
            var budgetInsights = new BudgetInsightsViewModel
            {
                AvgMonthlySpending = _unitOfWork.BudgetItemsRepository.AvgMonthlySpending(user),
                AmtOverOrUnder = _unitOfWork.BudgetItemsRepository.AmtOverOrUnderBudget(user),
                MostCommonCategory = _unitOfWork.BudgetItemsRepository.MostCommonCategory(user),
                CategoryTimes = _unitOfWork.BudgetItemsRepository.HowManyTimesCatOccur(_unitOfWork.BudgetItemsRepository.MostCommonCategory(user), user),
                NumMonthsUnderBudget = _unitOfWork.BudgetItemsRepository.NumMonthsUnderBudget(user),
                TotalMonths = _unitOfWork.BudgetItemsRepository.GetBudgetMonthsList(user).Count,
                TotalSpentFirstMonth = _unitOfWork.BudgetItemsRepository.TotalSpentByMonth(DateTime.Now.Month, user),
                TotalSpentSecondMonth = _unitOfWork.BudgetItemsRepository.TotalSpentByMonth((DateTime.Now.Month - 1), user),
                HighSaveCat = highestSavings.Key,
                HighSaveAmt = highestSavings.Value
            };

            if(budgetInsights.AmtOverOrUnder == 0)
            {
                ViewBag.NotEnoughInfoMsg = "If multiple insights are showing a 'No Information' message, " +
                    "there may not be enough budget data to calculate budget insights. This often happens when a new " +
                    "budget has just been created or if a budget does not have any transactions added.";

            }

            var budgetInsightsWithCategoryInsights = CalculateCategoryInsights(budgetInsights, User.Identity.Name);

            if (budgetInsightsWithCategoryInsights.BudgetInsightsByCategoryList.Count == 0)
            {
                ViewBag.HasCategoryInsights = false;
                return View(budgetInsightsWithCategoryInsights);
            }

            ViewBag.CategoryList = budgetInsightsWithCategoryInsights.BudgetInsightsByCategoryList.Select(x => x.CategoryType).ToList();
            var listOfCategoryNames = new List<string>();
            foreach (var category in ViewBag.CategoryList)
            {
                switch (category)
                {
                    case 1:
                        listOfCategoryNames.Add("Food | Grocery");
                        break;
                    case 2:
                        listOfCategoryNames.Add("Housing");
                        break;
                    case 3:
                        listOfCategoryNames.Add("Bills | Payments");
                        break;
                    case 4:
                        listOfCategoryNames.Add("Entertainment");
                        break;
                    case 5:
                        listOfCategoryNames.Add("Gas | Auto");
                        break;
                    case 6:
                        listOfCategoryNames.Add("Miscellaneous");
                        break;
                }
            }

            ViewBag.ListOfCategoryNames = listOfCategoryNames;

            return View(budgetInsightsWithCategoryInsights);
        }

        public BudgetInsightsViewModel CalculateCategoryInsights(BudgetInsightsViewModel budgetInsights, string user)
        {
            var monthListForUser = _unitOfWork.BudgetItemsRepository.GetBudgetMonthNumbersList(user);

            for (var budgetType = 1; budgetType < 7; budgetType++)
            {
                var insights = new BudgetInsightsByCategory
                {
                    CategoryType = budgetType,
                    AvgPurchasePrice = _unitOfWork.BudgetItemsRepository.AveragePurchasePriceByCat(monthListForUser, user, budgetType),
                    AvgSpentPerMonth = _unitOfWork.BudgetItemsRepository.AverageMonthlySpendingByCat(monthListForUser, user, budgetType),
                    AvgOverUnderPerCategory = _unitOfWork.BudgetItemsRepository.AvgOverUnderByCat(monthListForUser, user, budgetType),
                    NumMonthsUnderBudgetPerCat = _unitOfWork.BudgetItemsRepository.NumMonthsUnderBudgetByCat(monthListForUser, user, budgetType)
                };
                budgetInsights.BudgetInsightsByCategoryList.Add(insights);
            }

            return budgetInsights;
        }

        public IActionResult ExportExcel()
        {
            ViewBag.MonthsWithBudgetList = _unitOfWork.BudgetItemsRepository.GetBudgetMonthNumbersList(User.Identity.Name);
            return View();
        }

        [HttpGet]
        public FileContentResult ExportToExcel(int month)
        {
            var title = month > 12 ? "Budget Data" : "Budget Data for " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            var exportToExcelViewModel = _unitOfWork.BudgetItemsRepository.GenerateExportToExcelViewModel(User.Identity.Name, month);
            List<TransactionData> transactionDataList = exportToExcelViewModel.TransactionDataList;
            byte[] fileContent = ExportToExcelHelper.ExportExcel(transactionDataList, title, true);
            return File(fileContent, ExportToExcelHelper.ExcelContentType, "BudgetData.xlsx");
        }
    }

}
