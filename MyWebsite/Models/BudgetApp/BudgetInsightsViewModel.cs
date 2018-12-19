using System.Collections.Generic;

namespace MyWebsite.Models.BudgetApp
{
    public class BudgetInsightsViewModel
    {
        public float AvgMonthlySpending { get; set; }
        public float AmtOverOrUnder { get; set; }
        public string MostCommonCategory { get; set; }
        public int CategoryTimes { get; set; }
        public int NumMonthsUnderBudget { get; set; }
        public int TotalMonths { get; set; }
        public float TotalSpentFirstMonth { get; set; }
        public float TotalSpentSecondMonth { get; set; }
        public string HighSaveCat { get; set; }
        public float HighSaveAmt { get; set; }
        public List<BudgetInsightsByCategory> BudgetInsightsByCategory { get; set; }
    }

    public class BudgetInsightsByCategory
    {
        public int CategoryType { get; set; }
        public float AvgSpentPerMonth { get; set; }
        public float AvgOverUnderPerCategory { get; set; }
        public int NumMonthsUnderBudgetPerCat { get; set; }
        public float AvgPurchasePrice { get; set; }
    }
}
