﻿using Microsoft.AspNetCore.Mvc;
using MyWebsite.Data.Interfaces;
using MyWebsite.Models.BudgetProject;
using System;
using System.Collections.Generic;

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
            if (success)
                ViewBag.SuccessLimitsUpdated = "Budget limits successfully updated!";

            ViewBag.EmptyBudget = false;
            var currentMonth = DateTime.Now.Month;
            var budget = _unitOfWork.BudgetRepository.GetCurrentBudget(User.Identity.Name, currentMonth);

            foreach (var month in budget)
            {
                if (month.Month != currentMonth)
                {
                    ViewBag.EmptyBudget = true;
                    ViewBag.HasPastBudget = false;
                    var pastBudget = _unitOfWork.BudgetRepository.BudgetExistsByUser(User.Identity.Name);
                    if (pastBudget)
                        ViewBag.HasPastBudget = true;
                }
            }

            if (!ViewBag.EmptyBudget)
            {
                var budgetTotals = new List<float>();
                for (var i = 1; i < 7; i++)
                {
                    budgetTotals.Add(_unitOfWork.BudgetItemsRepository.TotalSpentByBudgetCategory(currentMonth, User.Identity.Name, i));
                }

                ViewBag.BudgetTotals = budgetTotals;
            }

            return View(budget);
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
                _unitOfWork.BudgetItemsRepository.Add(budgetItems);
                _unitOfWork.Complete();
                ViewBag.SuccessTransactionAdded = ($"Transaction Added! {budgetItems.Description}: ${budgetItems.Cost}");
            }

            var budget = _unitOfWork.BudgetItemsRepository.GetBudgetItemsListByMonth(User.Identity.Name, DateTime.Now.Month);

            return View(budget);
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
        public ActionResult Edit(int id, [Bind("Id,Email,GroceryLimit,HousingLimit,EntLimit,BillsLimit,GasLimit,MiscLimit, Month")] Budget budget)
        {
            if (id != budget.Id)
                return NotFound();
            _unitOfWork.BudgetRepository.Add(budget);
            _unitOfWork.Complete();

            return RedirectToAction("Index", new { userName = User.Identity.Name, success = true });
        }

        //Delete Specific Transaction
        public IActionResult DeleteTransaction(int? id, string returnTo)
        {
            var budgetTransaction = _unitOfWork.BudgetItemsRepository.GetTransactionById(Convert.ToInt32(id));
            if (budgetTransaction != null)
            {
                _unitOfWork.BudgetItemsRepository.RemoveItem(budgetTransaction);
                _unitOfWork.Complete();

                return RedirectToAction(returnTo, new { deleted = true, description = budgetTransaction.Description });
            }

            return RedirectToAction(returnTo, new { deleted = false });
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
            return View();
        }
    }

}
