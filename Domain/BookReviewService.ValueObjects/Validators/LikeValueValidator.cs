using BookReviewService.ValueObjects.Base;
using BookReviewService.ValueObjects.Exceptions;

namespace BookReviewService.ValueObjects.Validators;

public class LikeValueValidator : IValidator<int>
{
    public static int MIN_VALUE => 1;
    public static int MAX_VALUE => 10;

    public void Validate(int value)
    {
        if (value < MIN_VALUE || value > MAX_VALUE)
            throw new ArgumentOutOfRangeException(nameof(value), value, $"Like value must be between {MIN_VALUE} and {MAX_VALUE}.");
    }
}