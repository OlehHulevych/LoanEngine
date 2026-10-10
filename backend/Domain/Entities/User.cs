using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public int CreditScore { get; private set; }
    public decimal MonthlyIncome { get; private set; }    
    public EmploymentStatus EmploymentStatus { get; private set; }

    protected User()
    {
        FullName = string.Empty;
        Email = string.Empty;
    }

    public User(string fullName, string email, int creditScore,
        decimal monthlyIncome, EmploymentStatus employmentStatus)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (creditScore is < 300 or > 850)
            throw new ArgumentException("Credit score must be between 300 and 850.", nameof(creditScore));

        if (monthlyIncome < 0)
            throw new ArgumentException("Monthly income cannot be negative.", nameof(monthlyIncome));

        FullName = fullName;
        Email = email;
        CreditScore = creditScore;
        MonthlyIncome = monthlyIncome;
        EmploymentStatus = employmentStatus;
    }
}