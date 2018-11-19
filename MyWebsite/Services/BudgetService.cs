using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebsite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using MyWebsite.Models.BudgetProject;
using System.Linq.Dynamic;
using System.Reflection;

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

        public float GetBudgetLimitByLimitType(int type, int budgetMonth, string user)
        {
            var budget = _context.Budget.Where(x => x.Email == user).Where(x => x.Month == budgetMonth);

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

        public float GetTotalBudgetLimitByMonth(int month, string user)
        {
            var grocery = GetBudgetLimitByLimitType(1, month, user);
            var housing = GetBudgetLimitByLimitType(2, month, user);
            var bills = GetBudgetLimitByLimitType(3, month, user);
            var ent = GetBudgetLimitByLimitType(4, month, user);
            var gas = GetBudgetLimitByLimitType(5, month, user);
            var misc = GetBudgetLimitByLimitType(6, month, user);

            return (grocery + housing + bills + ent + gas + misc);
        }

        public int TotalSpentByMonth(int month, string user)
        {
            var totalSpent = _context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month)
                .Select(x => x.Cost).Sum();

            return Convert.ToInt32(totalSpent);
        }

        public float TotalSpentThisMonth(int month, string user)
        {
            float sum = 0;

            for (var type = 1; type < 7; type++)
            {
                foreach (var item in _context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month)
                    .Where(x => Convert.ToInt32(x.TypeOfBudget) == type))
                {
                    sum += item.Cost;
                }
            }

            return sum;
        }

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

        public List<string> GetBudgetMonthsList(string user)
        {
            var pastBudgetTransactions = _context.BudgetItems.Where(x => x.Email == user).ToList();

            var hasBudgetMonths = new List<string>();
            foreach (var transaction in pastBudgetTransactions)
            {
                switch (transaction.Month)
                {
                    case 1:
                        hasBudgetMonths.Add("January");
                        break;

                    case 2:
                        hasBudgetMonths.Add("February");
                        break;

                    case 3:
                        hasBudgetMonths.Add("March");
                        break;

                    case 4:
                        hasBudgetMonths.Add("April");
                        break;

                    case 5:
                        hasBudgetMonths.Add("May");
                        break;

                    case 6:
                        hasBudgetMonths.Add("June");
                        break;

                    case 7:
                        hasBudgetMonths.Add("July");
                        break;

                    case 8:
                        hasBudgetMonths.Add("August");
                        break;

                    case 9:
                        hasBudgetMonths.Add("September");
                        break;

                    case 10:
                        hasBudgetMonths.Add("October");
                        break;

                    case 11:
                        hasBudgetMonths.Add("November");
                        break;

                    case 12:
                        hasBudgetMonths.Add("December");
                        break;
                }
            }

            return hasBudgetMonths;
        }
    }
}
