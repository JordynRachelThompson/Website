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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public float GroceryLimit { get; set; }
        public float RentLimit { get; set; }
        public float EntertainmentLimit { get; set; }
        public float BillsLimit { get; set; }
        public float GasLimit { get; set; }
        public float MiscLimit { get; set; }
        public Type TypeOfBudget { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }

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


