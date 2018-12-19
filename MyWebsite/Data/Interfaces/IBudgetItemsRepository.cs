using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebsite.Models.BudgetApp;

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
        float AvgMonthlySpending(string user);
        float AmtOverOrUnderBudget(string user);
        string MostCommonCategory(string user);
        int HowManyTimesCatOccur(string category, string user);
        int NumMonthsUnderBudget(string user);
        Dictionary<string, float> HighestSavings(string user);
        List<int> GetBudgetMonthNumbersList(string user);
        float AverageMonthlySpendingByCat(List<int> months, string user, int budgetType);
        float AveragePurchasePriceByCat(List<int> months, string user, int budgetType);
        float AvgOverUnderByCat(List<int> months, string user, int budgetType);
        int NumMonthsUnderBudgetByCat(List<int> months, string user, int budgetType);
    }
}
