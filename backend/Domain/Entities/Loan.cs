using Domain.Common;
using Domain.Enums;
using Domain.Services;

namespace Domain.Entities;

public class Loan:BaseEntity
{
    public Guid UserId { get; private set; }
    public decimal Principal { get; private set; } 
    public decimal AnnualInterestRate { get; private set; } 
    public int TermMonths { get; private set; } 
    public DateTimeOffset? DisbursedAt { get; private set; }
    public DateTimeOffset? ApprovedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public decimal MonthlyEmi { get; private set; }
    
    public LoanStatus Status { get; private set; }
    private readonly List<Installment> _installments = new();
    public IReadOnlyCollection<Installment> Installments => _installments.AsReadOnly();

    protected Loan()
    {
        UserId = Guid.Empty;
        Principal = 0;
        AnnualInterestRate = 0;
        TermMonths = 0;
        Status = LoanStatus.Submitted;
        MonthlyEmi = 0;

    }

    public Loan(Guid userId, decimal principal, decimal annualInterestRate, int termMonths)
    {
        if(annualInterestRate<0) throw new ArgumentException("cannot be negative", nameof(annualInterestRate));
        if (principal <= 0) throw new ArgumentException("Principal must be greater than zero.", nameof(principal));
        if (termMonths <= 0) throw new ArgumentException("Term months must be positive.", nameof(termMonths));

        UserId = userId;
        Principal = principal;
        AnnualInterestRate = annualInterestRate;
        TermMonths = termMonths;
        Status = LoanStatus.Submitted;
        MonthlyEmi = EmiCalculator.CalculateMonthlyEmi(Principal, annualInterestRate, termMonths);
    }
    
    public void Approve()
    {
        if (Status != LoanStatus.Submitted)
            throw new InvalidOperationException($"Cannot approve a loan in '{Status}' status.");

        Status = LoanStatus.Approved;
        ApprovedAt = DateTimeOffset.UtcNow;
    }

    // 2. Reject
    public void Reject(string reason)
    {
        if (Status != LoanStatus.Submitted)
            throw new InvalidOperationException($"Cannot reject a loan in '{Status}' status.");

        Status = LoanStatus.Rejected;
    }

    public void Disburse()
    {
        
        
        if (Status != LoanStatus.Approved)
            throw new InvalidOperationException($"Cannot disburse funds for an unapproved loan (Current: '{Status}').");

        Status = LoanStatus.Disbursed;
        DisbursedAt = DateTimeOffset.UtcNow;
    }

    public void MarkCompleted()
    {
        // 1. Validate loan status first
        if (Status != LoanStatus.Disbursed)
            throw new InvalidOperationException("Only disbursed loans can be completed.");

        // 2. Validate installments exist
        if (!_installments.Any())
            throw new InvalidOperationException("Cannot complete loan: No installments exist.");

        // 3. Validate all installments are paid
        if (_installments.Any(i => i.Status != InstallmentStatus.Paid))
            throw new InvalidOperationException("Cannot complete loan: Not all installments are paid.");

        Status = LoanStatus.Completed;
        CompletedAt = DateTimeOffset.UtcNow;
    }

    public void AddInstallment(Installment installment)
    {
        _installments.Add(installment);
    }
}