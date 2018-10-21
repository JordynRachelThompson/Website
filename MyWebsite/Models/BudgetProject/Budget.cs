using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebsite.Models.BudgetProject
{
    public class Budget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public float GroceryLimit { get; set; } = 0;
        public float HousingLimit { get; set; } = 0;
        public float EntLimit { get; set; } = 0;
        public float BillsLimit { get; set; } = 0;
        public float GasLimit { get; set; } = 0;
        public float MiscLimit { get; set; } = 0;
        public int Month { get; set; }
        public List<BudgetItems> BudgetTransactions = new List<BudgetItems>();

    }

    public enum BudgetCategory
    {
        Grocery = 1,
        Housing = 2,
        Entertainment = 3,
        Bills = 4,
        Gas = 5,
        Misc = 6
    }

    public class BudgetItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }
        public DateTime Date { get; set; }
        public int Month {get; set;}
        public BudgetCategory TypeOfBudget { get; set; }

    }
}



