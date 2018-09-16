using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebsite.Models
{
    public class BudgetItems
    {
        [Key]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public List<Entries> GroceryItems { get; set; }
        public List<Entries> GroceryItem { get; set; }
        public List<Entries> RentItem {get; set; }
        public List<Entries> EntertainmentItem { get; set; }
        public List<Entries> BillItem { get; set; }
        public List<Entries> GasItem { get; set; }
        public List<Entries> MiscItem { get; set; }

    }

    public class Entries
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
    }
}