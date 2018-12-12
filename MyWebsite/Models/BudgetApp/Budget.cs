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
}



