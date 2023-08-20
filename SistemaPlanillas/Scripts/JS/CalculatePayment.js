document.addEventListener('DOMContentLoaded', function () {
    // Verifica el tipo de pago de la vista
    if (isMonthlyPayment) {
        // Código para pagos mensuales
        var selectDias = document.getElementById('selectDias');
        var salaryInput = document.getElementById('salary');
        var deductionsInput = document.getElementById('deductions');
        var paymentsExtraordinaryInput = document.getElementById('paymentsExtraordinary');
        var calculatedAmountInput = document.getElementById('calculatedAmount');

        selectDias.addEventListener('change', function () {
            var daysWorked = parseInt(selectDias.value);
            var salary = parseFloat(salaryInput.value);
            var deductions = parseFloat(deductionsInput.value);
            var paymentsExtraordinary = parseFloat(paymentsExtraordinaryInput.value);

            var paymentAmount = (salary / 22) * daysWorked + paymentsExtraordinary - deductions;
            var paymentAmountFormatted = paymentAmount.toFixed(2).replace(".", ",");

            calculatedAmountInput.value = paymentAmountFormatted;
        });
    } else {
        // Código para pagos por horas
        var hoursWorkedInput = document.getElementById('hoursWorked');
        var hourlySalaryInput = document.getElementById('hourlySalary');
        var hourlyDeductionsInput = document.getElementById('hourlyDeductions');
        var hourlyPaymentsExtraordinaryInput = document.getElementById('hourlyPaymentsExtraordinary');
        var hourlyCalculatedAmountInput = document.getElementById('hourlyCalculatedAmount');

        hoursWorkedInput.addEventListener('change', function () {
            var hoursWorked = parseInt(hoursWorkedInput.value);
            var hourlySalary = parseFloat(hourlySalaryInput.value);
            var hourlyDeductions = parseFloat(hourlyDeductionsInput.value);
            var hourlyPaymentsExtraordinary = parseFloat(hourlyPaymentsExtraordinaryInput.value);

            // Aplicar la lógica de pago doble después de 9 horas
            var regularPayment = hourlySalary * hoursWorked;
            var overtimePayment = 0;
            if (hoursWorked > 9) {
                var overtimeHours = hoursWorked - 9;
                overtimePayment = hourlySalary * 2 * overtimeHours; // Pago doble por horas extra
                regularPayment = hourlySalary * 9; // Pago regular hasta las primeras 9 horas
            }

            var totalPayment = regularPayment + overtimePayment + hourlyPaymentsExtraordinary - hourlyDeductions;
            var paymentAmountFormatted = totalPayment.toFixed(2).replace(".", ",");

            hourlyCalculatedAmountInput.value = paymentAmountFormatted;
        });

    }
});