using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaPlanillas.Models
{
    public class PayrollViewModel
    {
        public string UserDepart { get; set; }
        public Users User { get; set; }
        public int IdUserLogin { get; set; }
        public int IdUserEdit { get; set; }
        public string SalaryType { get; set; }
        public decimal Salary { get; set; }
        public decimal Deductions { get; set; }
        public decimal PaymentsExtraordinary { get; set; }
    }
}