using Microsoft.EntityFrameworkCore.Extensions.Internal;
using MyWebsite.Data;
using MyWebsite.Models.BudgetProject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MyWebsite.Services
{
    public class BudgetService
    {
        private readonly ApplicationDbContext _context;

        public BudgetService(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool BudgetExists(int id)
        {
            return _context.Budget.Any(e => e.Id == id);
        }

        //Budget limit by type of budget
        public float GetBudgetLimitByLimitType(int type, int budgetMonth, string user)
        {
            var budget = _context.Budget.Where(x => x.Email == user).Where(x => x.Month == budgetMonth).ToList();

            foreach (var item in budget)
            {
                switch (type)
                {
                    case 1:
                        return item.GroceryLimit;
                    case 2:
                        return item.HousingLimit;
                    case 3:
                        return item.BillsLimit;
                    case 4:
                        return item.EntLimit;
                    case 5:
                        return item.GasLimit;
                    case 6:
                        return item.MiscLimit;
                }
            }

            return 0;
        }

        //Budget limit by month
        public float GetTotalBudgetLimitByMonth(int month, string user)
        {
            float sum = 0;

            for (var i = 1; i < 7; i++)
            {
                var budgetLimit = GetBudgetLimitByLimitType(i, month, user);
                sum += budgetLimit;
            }

            return sum;
        }

        //Total spent by month
        public float TotalSpentByMonth(int month, string user)
        {
            return _context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month)
                .Select(x => x.Cost).Sum();
        }


        //public float TotalSpentThisMonth(int month, string user)
        //{
        //    float sum = 0;

        //    for (var type = 1; type < 7; type++)
        //    {
        //        foreach (var item in _context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month)
        //            .Where(x => Convert.ToInt32(x.TypeOfBudget) == type))
        //        {
        //            sum += item.Cost;
        //        }
        //    }

        //    return sum;
        //}

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

        public void SetBudgetLimitToPastLimit(string user, Budget currentBudget)
        {
            var previousBudget = _context.Budget.Where(x => x.Email == user).Select(x => x).FirstOrDefault();

            if (previousBudget != null)
            {
                currentBudget.GroceryLimit = previousBudget.GroceryLimit;
                currentBudget.HousingLimit = previousBudget.HousingLimit;
                currentBudget.BillsLimit = previousBudget.BillsLimit;
                currentBudget.EntLimit = previousBudget.EntLimit;
                currentBudget.GasLimit = previousBudget.GasLimit;
                currentBudget.MiscLimit = previousBudget.MiscLimit;

                _context.Add(currentBudget);
                _context.SaveChanges();
            }
        }

        //Return list of months where user set a budget
        public List<string> GetBudgetMonthsList(string user)
        {
            return _context.BudgetItems.Where(x => x.Email == user).Select(x => DateTimeFormatInfo.CurrentInfo.GetMonthName(x.Month)).Distinct().ToList();
            
        }
    }
}
