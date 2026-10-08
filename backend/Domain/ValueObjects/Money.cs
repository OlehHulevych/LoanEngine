namespace Domain.ValueObjects;

public record Money(decimal Amount)
{
   private readonly decimal _amount =
      Math.Round(
         Amount >= 0 ? Amount : throw new ArgumentOutOfRangeException(nameof(Amount), "Value cannot be negative"), 2,
         MidpointRounding.AwayFromZero);

   public decimal Amount
   {
      get => _amount;
      init => _amount = value < 0
         ? throw new ArgumentOutOfRangeException(nameof(value), "Value cannout be negative")
         : Math.Round(value, 2, MidpointRounding.AwayFromZero);
   }
} 