using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebsite.Models.BudgetApp
{
    public class ExportToExcelViewModel
    {
        public List<TransactionData> TransactionDataList { get; set; } = new List<TransactionData>();
    }

    public class TransactionData
    {
        public string Month { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
    }
}
