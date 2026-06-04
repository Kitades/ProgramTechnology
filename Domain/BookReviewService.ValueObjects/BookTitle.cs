using BookReviewService.ValueObjects.Base;
using BookReviewService.ValueObjects.Validators;

namespace BookReviewService.ValueObjects;

public class BookTitle(string title) : ValueObject<string>(new BookTitleValidator(), title);