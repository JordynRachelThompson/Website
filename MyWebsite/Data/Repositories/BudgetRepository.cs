using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Data.Interfaces;
using MyWebsite.Models.BudgetProject;

namespace MyWebsite.Data.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _context;

        public BudgetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool BudgetExists(int id)
        {
            return _context.Budget.Any(x => x.Id == id);
        }

        public bool BudgetExistsByUser(string user)
        {
            return _context.Budget.Any(x => x.Email == user);
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

        public void SetBudgetLimitToPastLimit(Budget currentBudget)
        {
            var user = currentBudget.Email;
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

        //Set new budget limits
        public void SetNewBudgetLimits(Budget budgetLimits)
        {
            _context.Add(budgetLimits);
            _context.SaveChanges();
        }

        public List<Budget> GetCurrentBudget(string user, int currentMonth)
        {
            return _context.Budget.Where(x => x.Email == user && x.Month == currentMonth).ToList();
        }

        public Budget GetBudgetById(int id)
        {
            return  _context.Budget.SingleOrDefault(x => x.Id == id);
        }

        public void Add(Budget budget)
        {
            _context.Budget.Add(budget);
        }
    }
}
