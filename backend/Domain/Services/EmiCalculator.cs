namespace Domain.Services;

public static class EmiCalculator
{
    public static decimal CalculateMonthlyEmi(decimal principal, decimal annualInterestRate, int termMonths)
    {
        if (principal <= 0)
            throw new ArgumentException("Principal must be greater than zero.", nameof(principal));

        if (termMonths <= 0)
            throw new ArgumentException("Term months must be positive.", nameof(termMonths));

        if (annualInterestRate == 0)
        {
            return Math.Round(principal / termMonths, 2, MidpointRounding.AwayFromZero);
        }

        decimal monthlyRate = annualInterestRate / (12 * 100);
        double rateDouble = (double)monthlyRate;
        double factor = Math.Pow(1 + rateDouble, termMonths);
        decimal emi = principal * monthlyRate * (decimal)(factor / (factor - 1));
        return Math.Round(emi, 2, MidpointRounding.AwayFromZero);
    }
}