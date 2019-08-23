using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioWebsite.Models.BudgetApp
{
    public class BudgetItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        public float Cost { get; set; }
        public DateTime Date { get; set; }
        public int Month { get; set; }
        public BudgetCategory TypeOfBudget { get; set; }
    }

    public enum BudgetCategory
    {
        Grocery = 1,
        Housing,
        Entertainment,
        Bills,
        Gas,
        Misc
    }
}
