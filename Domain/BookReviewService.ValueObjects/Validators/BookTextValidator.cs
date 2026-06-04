using BookReviewService.ValueObjects.Base;
using BookReviewService.ValueObjects.Exceptions;

namespace BookReviewService.ValueObjects.Validators;

public class BookTextValidator : IValidator<string>
{
    // Текст книги может быть длинным, минимальная длина 1
    public void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(nameof(value));
        // Дополнительный лимит при желании, например 10000
        const int maxLength = 10000;
        if (value.Length > maxLength)
            throw new ArgumentLongValueException(nameof(value), value, maxLength);
    }
}