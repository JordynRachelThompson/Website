using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebsite.Models.BudgetProject;

namespace MyWebsite.Data.Interfaces
{
    public interface IBudgetItemsRepository
    {
        float TotalSpentByMonth(int month, string user);
        float TotalSpentByBudgetCategory(int month, string user, int type);
        List<string> GetBudgetMonthsList(string user);
        List<BudgetItems> GetBudgetItemsListByMonth(string user, int month);
        void Add(BudgetItems budgetItems);
        List<BudgetItems> ReturnBudgetItemsListForUser(string user);
        BudgetItems GetTransactionById(int id);
        void RemoveItem(BudgetItems budgetItem);
    }
}
