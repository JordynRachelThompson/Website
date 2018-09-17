using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebsite.Models
{
    public class BudgetTransactions
    {
        [Key]
        public int TransactionId { get; set; }
        public Type TypeOfBudget { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }

        [ForeignKey("BudgetEntries")]
        public int BudgetRefId { get; set; }
        public BudgetEntries BudgetEntries { get; set; }
    }

    public enum Type
    {
        None = 0,
        Grocery = 1,
        Rent = 2,
        Entertainment = 3,
        Bills = 4,
        Gas = 5,
        Misc = 6
    }
}
