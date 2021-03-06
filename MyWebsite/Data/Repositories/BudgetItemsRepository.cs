﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PortfolioWebsite.Data.Interfaces;
using PortfolioWebsite.Models.BudgetApp;

namespace PortfolioWebsite.Data.Repositories
{
    public class BudgetItemsRepository : IBudgetItemsRepository
    {
        private readonly ApplicationDbContext _context;

        public BudgetItemsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<BudgetItems> ReturnBudgetItemsListForUser(string user)
        {
            return _context.BudgetItems.Where(x => x.Email == user).ToList();
        }

        //Total spent by month
        public float TotalSpentByMonth(int month, string user)
        {
            return _context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month).Select(x => x.Cost)
                .Sum();
        }

        //Total spent by category (type)
        public float TotalSpentByBudgetCategory(int month, string user, int type)
        {
            float sum = 0;

            foreach (var item in _context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month)
                .Where(x => Convert.ToInt32(x.TypeOfBudget) == type))
            {
                sum += item.Cost;
            }

            return sum;
        }

        //Return list of months where user set a budget
        public List<string> GetBudgetMonthsList(string user)
        {
            return _context.BudgetItems.Where(x => x.Email == user).OrderBy(x => x.Month)
                .Select(x => DateTimeFormatInfo.CurrentInfo.GetMonthName(x.Month)).Distinct().ToList();
        }

        public List<int> GetBudgetMonthNumbersList(string user)
        {
            return _context.BudgetItems.Where(x => x.Email == user).OrderBy(x => x.Month)
                .Select(x => x.Month).Distinct().ToList();
        }

        public List<BudgetItems> GetBudgetItemsListByMonth(string user, int month)
        {
            return _context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month).ToList();
        }

        public void Add(BudgetItems budgetItems)
        {
            _context.BudgetItems.Add(budgetItems);
        }

        public BudgetItems GetTransactionById(int id)
        {
            return _context.BudgetItems.SingleOrDefault(x => x.TransactionId == id);
        }

        public void RemoveItem(BudgetItems budgetItem)
        {
            _context.BudgetItems.Remove(budgetItem);
        }

        public float AvgMonthlySpending(string user)
        {
            float total = 0;
            var count = 0;

            var transactionList = _context.BudgetItems.Where(x => x.Email == user).Select(x => x.Cost).ToList();

            foreach (var purchase in transactionList)
            {
                total += purchase;
                count++;
            }

            return total / count;
        }

        public float AmtOverOrUnderBudget(string user)
        {
            var monthListForUser = GetBudgetMonthNumbersList(user);

            if (!monthListForUser.Any()) return 0.00f;

            float totalBudget = 0;
            float totalSpent = 0;

            var budgetRepo = new BudgetRepository(_context);

            foreach (var month in monthListForUser)
            {
                totalBudget += budgetRepo.GetTotalBudgetLimitByMonth(month, user);
                totalSpent += TotalSpentByMonth(month, user);
            }

            return totalBudget - totalSpent;
        }

        public string MostCommonCategory(string user)
        {
            var item = _context.BudgetItems
                .Where(x => x.Email == user)
                .GroupBy(x => x.TypeOfBudget)
                .OrderByDescending(gb => gb.Count())
                .Select(g => g.Key)
                .FirstOrDefault().ToString();

            return item;
        }

        public int HowManyTimesCatOccur(string category, string user)
        {
            return _context.BudgetItems.Count(x => x.Email == user && x.TypeOfBudget.ToString() == category);
        }

        public int NumMonthsUnderBudget(string user)
        {

            List<string> monthList = GetBudgetMonthsList(user);
            if (!monthList.Any()) return 0;

            var monthListNumbers = new List<int>();
            foreach (var month in monthList)
                monthListNumbers.Add(DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month);

            var underBudget = 0;

            var budgetRepo = new BudgetRepository(_context);

            foreach (var month in monthListNumbers)
            {
                var difference = budgetRepo.GetTotalBudgetLimitByMonth(month, user) - TotalSpentByMonth(month, user);

                if (difference > 0) underBudget++;
            }

            return underBudget;
        }

        public Dictionary<string, float> HighestSavings(string user)
        {
            var category = 0;
            var mostSaved = 0.0f;
            var categoryStr = "";

            var budgetRepo = new BudgetRepository(_context);

            for (var month = 1; month < 13; month++)
            {
                for (var budgetType = 1; budgetType < 7; budgetType++)
                {
                    var limit = budgetRepo.GetBudgetLimitByLimitType(budgetType, month, user);
                    var spent = TotalSpentByBudgetCategory(month, user, budgetType);
                    var difference = limit - spent;
                    if (difference > mostSaved)
                    {
                        category = budgetType;
                        mostSaved = difference;
                    }
                }
            }

            var categoryUnderBudget = new Dictionary<string, float>();

            switch (category)
            {
                case 1:
                    categoryStr = "Grocery | Restaurant";
                    break;
                case 2:
                    categoryStr = "Mortgage | Rent";
                    break;
                case 3:
                    categoryStr = "Bills | Payments";
                    break;
                case 4:
                    categoryStr = "Entertainment";
                    break;
                case 5:
                    categoryStr = "Gas | Auto";
                    break;
                case 6:
                    categoryStr = "Miscellaneous";
                    break;
            }

            if (mostSaved > 0)
                categoryUnderBudget.Add(categoryStr, mostSaved);
            else
                categoryUnderBudget.Add("No Information", 0);

            return categoryUnderBudget;
        }

        public int NumMonthsUnderBudgetByCat(List<int> months, string user, int budgetType)
        {
            var budgetRepo = new BudgetRepository(_context);
            int numMonthsUnderBudget = 0;

            foreach (var month in months)
            {
                if (budgetRepo.GetBudgetLimitByLimitType(budgetType, month, user) - TotalSpentByBudgetCategory(month, user, budgetType) > 0)
                    numMonthsUnderBudget += 1;
            }

            return numMonthsUnderBudget;
        }

        public float AvgOverUnderByCat(List<int> months, string user, int budgetType)
        {
            var monthListForUser = GetBudgetMonthNumbersList(user);

            if (!monthListForUser.Any()) return 0.00f;

            float totalBudgetByCat = 0;
            float totalSpent = 0;

            var budgetRepo = new BudgetRepository(_context);

            foreach (var month in monthListForUser)
            {
                totalBudgetByCat += budgetRepo.GetBudgetLimitByLimitType(budgetType, month, user);
                totalSpent += TotalSpentByBudgetCategory(month, user, budgetType);
            }

            return totalBudgetByCat - totalSpent;
        }

        public float AveragePurchasePriceByCat(List<int> months, string user, int budgetType)
        {
            float purchasePrices = 0.0f;
            var numberOfPurchases = 0;

            foreach (var month in months)
            {
                foreach (var purchase in GetBudgetItemsListByMonth(user, month))
                {
                    purchasePrices += purchase.Cost;
                    numberOfPurchases += 1;
                }
            }

            if (numberOfPurchases == 0 || purchasePrices < 0.1f) return 0.0f;

            return purchasePrices / numberOfPurchases;
        }

        public float AverageMonthlySpendingByCat(List<int> months, string user, int budgetType)
        {
            float totalSpent = 0.0f;
            int howManyMonths = months.Count;

            foreach (var month in months)
            {
                totalSpent += TotalSpentByBudgetCategory(month, user, budgetType);
            }

            if (howManyMonths == 0 || totalSpent < 0.1f) return 0.0f;

            return totalSpent / howManyMonths;
        }

        public ExportToExcelViewModel GenerateExportToExcelViewModel(string user, int month)
        {
            var exportToExcelViewModel = new ExportToExcelViewModel();
            var allPurchases = _context.BudgetItems.Where(x => x.Email == user).ToList();

            if (!allPurchases.Any()) return exportToExcelViewModel;

            //If month == 13 add all months to Excel
            var budgetData = month > 12
                ? _context.BudgetItems.Where(x => x.Email == user).ToList()
                : _context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month).ToList();
            
            foreach (var transaction in budgetData)
            {
                var transactionData = new TransactionData
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(transaction.Month),
                    Category = transaction.TypeOfBudget.ToString(), //deal with later
                    Description = transaction.Description,
                    Price = String.Format("{0:$#,##0.00;($#,##0.00);Zero}", transaction.Cost),
                    Date = transaction.Date.ToString("MM/dd/yyyy"),
                };

                exportToExcelViewModel.TransactionDataList.Add(transactionData);
            }

            foreach (var category in exportToExcelViewModel.TransactionDataList)
            {
                if (category.Category == "1")
                    category.Category = "Grocery/Food";
                if (category.Category == "2")
                    category.Category = "Housing";
                if (category.Category == "3")
                    category.Category = "Entertainment";
                if (category.Category == "4")
                    category.Category = "Bills/Payments";
                if (category.Category == "5")
                    category.Category = "Gas/Auto";
                if (category.Category == "6")
                    category.Category = "Miscellaneous";
            }

            return exportToExcelViewModel;
        }
    }
}
