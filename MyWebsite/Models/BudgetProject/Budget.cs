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
        public float GroceryLimit { get; set; }
        public float HousingLimit { get; set; }
        public float EntLimit { get; set; }
        public float BillsLimit { get; set; }
        public float GasLimit {get; set;}
        public float MiscLimit { get; set; }
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



