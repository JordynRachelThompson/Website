using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MyWebsite.Data.Interfaces;
using MyWebsite.Models.BudgetProject;

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
            return _context.BudgetItems.Where(x => x.Email == user).OrderBy(x => x.Month)
                .Select(x => DateTimeFormatInfo.CurrentInfo.GetMonthName(x.Month)).Distinct().ToList();
        }

        public List<BudgetItems> GetBudgetItemsListByMonth(string user, int month)
        {
            return _context.BudgetItems.Where(x => x.Email == user).Where(x => x.Month == month).ToList();
        }

        public void Add(BudgetItems budgetItems)
        {
            _context.Add(budgetItems);
        }

        public BudgetItems GetTransactionById(int id)
        {
            return _context.BudgetItems.SingleOrDefault(x => x.TransactionId == id);
        }

        public void RemoveItem(BudgetItems budgetItem)
        {
            _context.BudgetItems.Remove(budgetItem);
        }
    }
}
