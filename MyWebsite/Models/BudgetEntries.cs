using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebsite.Models
{
    public class BudgetEntries
    {
        [Key]
        public int BudgetId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public float GroceryLimit { get; set; }
        public float RentLimit { get; set; }
        public float EntertainmentLimit { get; set; }
        public float BillsLimit { get; set; }
        public float GasLimit { get; set; }
        public float MiscLimit { get; set; }
        public ICollection<BudgetTransactions> BudgetTransactions { get; set; }

    }

}


