using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortfolioWebsite.Models.BudgetApp;

namespace PortfolioWebsite.Data.Interfaces
{
    public interface IBudgetRepository
    {
        bool BudgetExists(int id);
        float GetBudgetLimitByLimitType(int type, int budgetMonth, string user);
        float GetTotalBudgetLimitByMonth(int month, string user);
        void SetBudgetLimitToPastLimit(Budget currentBudget);
        void SetNewBudgetLimits(Budget budgetLimits);
        Budget GetCurrentBudget(string user, int currentMonth);
        bool BudgetExistsByUser(string user);
        Budget GetBudgetById(int id);
        void Add(Budget budget);
        List<Budget> GetAllBudgetsForUser(string user);
        bool CurrentBudgetExistsByUser(string user, int currentMonth);
        void EditBudgetLimits(Budget budget);
    }
}
