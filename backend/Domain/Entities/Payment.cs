using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Payment:BaseEntity
{
    public Guid LoanId { get; private set; }
    public Guid? InstallmentId { get; private set; }
    public Money Amount { get; private set; }
    public DateTimeOffset PaymentDate { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public string TransactionReference { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }

    protected Payment()
    {
        LoanId = Guid.Empty;
        Amount = new Money(0);
        PaymentDate = DateTimeOffset.UtcNow;
        PaymentMethod = PaymentMethod.Cash;
        TransactionReference = null!;
        PaymentStatus = PaymentStatus.Pending;
    }

    public Payment(Guid loanId, decimal amount, PaymentMethod paymentMethod, string transactionReference, Guid? installmentId = null)
    {
        if (loanId == Guid.Empty)
            throw new ArgumentException("LoanId cannot be empty.", nameof(loanId));
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero.", nameof(amount));
        if (string.IsNullOrWhiteSpace(transactionReference))
            throw new ArgumentException("Transaction reference is required.", nameof(transactionReference));
        
        LoanId = loanId;
        Amount = new Money(amount);
        PaymentMethod = paymentMethod;
        TransactionReference = transactionReference;
        PaymentDate = DateTimeOffset.UtcNow;
        PaymentStatus = PaymentStatus.Pending;
        InstallmentId = installmentId;
    }

    public void MarkSuccessful()
    {
        if (PaymentStatus != PaymentStatus.Pending)
            throw new InvalidOperationException($"Cannot succeed payment with status '{PaymentStatus}'.");
        PaymentStatus = PaymentStatus.Success;
    }
    public void Refund()
    {
        if (PaymentStatus != PaymentStatus.Success)
            throw new InvalidOperationException($"Only successful payments can be refunded (Current: '{PaymentStatus}').");
        PaymentStatus = PaymentStatus.Refunded;
    }

    public void MarkFailed()
    {
        if (PaymentStatus != PaymentStatus.Pending)
            throw new InvalidOperationException($"Cannot fail payment with status '{PaymentStatus}'.");
        PaymentStatus = PaymentStatus.Failed;
    }
}