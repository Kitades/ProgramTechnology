using BookReviewService.ValueObjects.Base;
using BookReviewService.ValueObjects.Validators;

namespace BookReviewService.ValueObjects;

public class Username(string name) : ValueObject<string>(new UsernameValidator(), name);