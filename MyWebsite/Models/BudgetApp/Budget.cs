using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite.Models.BudgetApp
{
    public class Budget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Currency)]
        public float GroceryLimit { get; set; } = 0;
        [DataType(DataType.Currency)]
        public float HousingLimit { get; set; } = 0;
        [DataType(DataType.Currency)]
        public float EntLimit { get; set; } = 0;
        [DataType(DataType.Currency)]
        public float BillsLimit { get; set; } = 0;
        [DataType(DataType.Currency)]
        public float GasLimit { get; set; } = 0;
        [DataType(DataType.Currency)]
        public float MiscLimit { get; set; } = 0;
        public int Month { get; set; }
        public List<BudgetItems> BudgetTransactions = new List<BudgetItems>();
    }
}



