using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaPlanillas.Models.ViewModels
{
    public class MonthlyPaymentCompositeModel
    {
        public Payroll_history History { get; set; }
        public Monthly_payroll MonthlyPayroll { get; set; }
    }

    public class HourlyPaymentCompositeModel
    {
        public Payroll_history History { get; set; }
        public List<hourly_payroll> HourlyPayrolls { get; set; }
    }
}