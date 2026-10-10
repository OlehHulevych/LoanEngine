namespace Domain.ValueObjects;

public record Money
{
  public decimal Amount { get; }

  public Money(decimal amount)
  {
    if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Money value cannout be negative");
    Amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
  }

  public static Money Zero => new(0m);

  public static Money operator +(Money a, Money b) => new (a.Amount + b.Amount);
  public static Money operator -(Money a, Money b) => new(a.Amount - b.Amount);
  public override string ToString() => $"{Amount:F2}";
} 