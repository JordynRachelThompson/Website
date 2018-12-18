﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MyWebsite.Data.Interfaces;
using MyWebsite.Models.BudgetApp;

namespace MyWebsite.Data.Repositories
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
            return _context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month).Select(x => x.Cost).Sum();
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
            var months = _context.BudgetItems.Where(x => x.Email == user).OrderBy(x => x.Month)
                .Select(x => DateTimeFormatInfo.CurrentInfo.GetMonthName(x.Month)).Distinct().ToList();
            return months;
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
            List<int> monthList = _context.Budget.Where(x => x.Email == user).Select(x => x.Month).ToList();

            if (!monthList.Any()) return 0.00f;

            float totalBudget = 0;
            float totalSpent = 0;

            var budgetRepo = new BudgetRepository(_context);

            foreach (var month in monthList)
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
    }
}
