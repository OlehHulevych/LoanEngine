using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Installment:BaseEntity
{
    public Guid LoanId { get; private set; }
    public DateTimeOffset DueDate { get; private set; }
    public Decimal PrincipalAmount { get; private set; }
    public Decimal InterestAmount { get; private set; }
    public Decimal PenaltyAmount { get; private set; }
    public int InstallmentNumber { get; private set; }
    public InstallmentStatus Status { get; private set; }
    public DateTimeOffset? PaidAt { get; private set; }
    public decimal TotalDue => PrincipalAmount + InterestAmount + PenaltyAmount;

    protected Installment()
    {
        LoanId = Guid.Empty;
        DueDate = DateTimeOffset.UtcNow;
        PrincipalAmount = 0;
        InterestAmount = 0;
        PenaltyAmount = 0;
        Status = InstallmentStatus.Pending;
    }

    public Installment(Guid loanId, DateTimeOffset dueDate, decimal principalAmount, decimal interestAmount, int installmentNumber)
    {
        LoanId = loanId;
        DueDate = dueDate;
        PrincipalAmount = principalAmount;
        InterestAmount = interestAmount;
        Status = InstallmentStatus.Pending;
        InstallmentNumber = installmentNumber;

    }
    
    public void MarkOverdue(decimal lateFee)
    {
        if (Status != InstallmentStatus.Pending)
            return;

        Status = InstallmentStatus.Overdue;
        PenaltyAmount = lateFee;
    }
    public void MarkPaid(DateTimeOffset paidAt)
    {
        if (Status == InstallmentStatus.Paid)
            throw new InvalidOperationException("Installment is already paid.");

        Status = InstallmentStatus.Paid;
        PaidAt = paidAt;
    }
    
}