using BookReviewService.ValueObjects.Base;
using BookReviewService.ValueObjects.Validators;

namespace BookReviewService.ValueObjects;

public class BookName(string name) : ValueObject<string>(new BookNameValidator(), name);