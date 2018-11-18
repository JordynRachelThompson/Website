using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebsite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Models.BudgetProject;

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

        public int GetBudgetLimitByLimitType(string limitType, int budgetMonth, string user)
        {
            return Convert.ToInt32(_context.Budget.Where(x => x.Email == user).Where(x => x.Month == budgetMonth)
                .Select(x => limitType));
        }

        public int GetTotalBudgetLimitByMonth(int month, string user)
        {
            var grocery = GetBudgetLimitByLimitType("GroceryLimit", month, user);
            var housing = GetBudgetLimitByLimitType("HousingLimit", month, user);
            var bills = GetBudgetLimitByLimitType("BillsLimit", month, user);
            var ent = GetBudgetLimitByLimitType("EntLimit", month, user);
            var gas = GetBudgetLimitByLimitType("GasLimit", month, user);
            var misc = GetBudgetLimitByLimitType("MiscLimit", month, user);

            return (grocery + housing + bills + ent + gas + misc);
        }

        public int TotalSpentByMonth(int month, string user)
        {
            return Convert.ToInt32(_context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month).Select(x => x.Cost).Sum());
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
    }
}
