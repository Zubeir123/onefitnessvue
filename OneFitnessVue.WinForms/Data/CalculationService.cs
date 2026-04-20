using FitnessTimeGym.WinForms.Models;

namespace FitnessTimeGym.WinForms.Data;

public class CalculationService
{
    public int CalculateAge(DateTime dob)
    {
        var today = DateTime.Today;
        var age = today.Year - dob.Year;
        if (dob.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    public TaxCalculationResult CalculateTotal(decimal amount, decimal taxPercentage)
    {
        var taxAmount = (taxPercentage / 100m) * amount;
        return new TaxCalculationResult
        {
            Amount = amount,
            TaxPercentage = taxPercentage,
            TaxAmount = taxAmount,
            TotalAmount = amount + taxAmount
        };
    }
}
