using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebsite.Models
{
    public class BudgetLimit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int GroceryLimit { get; set; }
        public int RentLimit { get; set; }
        public int BillsLimit { get; set; }
        public int GasLimit { get; set; }
        public int EntertainmentLimit { get; set; }
        public int MiscLimit { get; set; }

    }
}


