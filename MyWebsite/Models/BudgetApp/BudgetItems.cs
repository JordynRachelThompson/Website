using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebsite.Models.BudgetProject
{
    public class BudgetItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
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
