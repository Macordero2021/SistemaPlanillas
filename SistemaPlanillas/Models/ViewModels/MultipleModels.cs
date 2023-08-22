using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaPlanillas.Models.ViewModels
{
    public class MultipleModels
    {
        public PayrollViewModel UsersWithInfo { get; set; }
        public List<MonthlyPaymentCompositeModel> MonthlyPayments { get; set; }
        public List<HourlyPaymentCompositeModel> HourlyPayments { get; set; }
    }
}