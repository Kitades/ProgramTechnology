using BookReviewService.Domain.Repositories.Abstractions.Base;

namespace BookReviewService.Domain.Repositories.Abstractions;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}