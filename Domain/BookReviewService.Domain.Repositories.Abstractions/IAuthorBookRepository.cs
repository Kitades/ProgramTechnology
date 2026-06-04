using BookReviewService.Domain.Repositories.Abstractions.Base;

namespace BookReviewService.Domain.Repositories.Abstractions;

public interface IAuthorBookRepository : IRepository<AuthorBook, Guid>
{
    Task<AuthorBook?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}