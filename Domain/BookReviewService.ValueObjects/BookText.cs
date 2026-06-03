using BookReviewService.ValueObjects.Base;
using BookReviewService.ValueObjects.Validators;

namespace BookReviewService.ValueObjects;

public class BookText(string text) : ValueObject<string>(new BookTextValidator(), text);