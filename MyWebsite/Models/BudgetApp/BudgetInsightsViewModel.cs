using System.Collections.Generic;

namespace MyWebsite.Models.BudgetApp
{
    public class BudgetInsightsViewModel
    {
        public float AvgMonthlySpending { get; set; } = 0;
        public float AmtOverOrUnder { get; set; } = 0;
        public string MostCommonCategory { get; set; } = "";
        public int CategoryTimes { get; set; } = 0;
        public int NumMonthsUnderBudget { get; set; } = 0;
        public int TotalMonths { get; set; } = 0;
        public float TotalSpentFirstMonth { get; set; } = 0;
        public float TotalSpentSecondMonth { get; set; } = 0;
        public string HighSaveCat { get; set; } = "";
        public float HighSaveAmt { get; set; } = 0;
        public List<BudgetInsightsByCategory> BudgetInsightsByCategoryList { get; set; } = new List<BudgetInsightsByCategory>();
    }

    public class BudgetInsightsByCategory
    {
        public int CategoryType { get; set; }
        public float AvgSpentPerMonth { get; set; } = 0;
        public float AvgOverUnderPerCategory { get; set; } = 0;
        public int NumMonthsUnderBudgetPerCat { get; set; } = 0;
        public float AvgPurchasePrice { get; set; } = 0;
    }
}
