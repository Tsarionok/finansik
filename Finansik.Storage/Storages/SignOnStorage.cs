using Finansik.Domain.UseCases.SignOn;
using Finansik.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

public class SignOnStorage(
    FinansikDbContext dbContext,
    IGuidFactory guidFactory) : ISignOnStorage
{
    public async Task<bool> IsLoginAlreadyUsed(string login, CancellationToken cancellationToken) => 
        await dbContext.Users.AnyAsync(u => u.Login == login, cancellationToken);

    public async Task<Guid> CreateUser(string login, byte[] salt, byte[] passwordHash, CancellationToken cancellationToken)
    {
        var userId = guidFactory.Create();
        await dbContext.Users.AddAsync(new User
        {
            Id = userId,
            Login = login,
            PasswordHash = passwordHash,
            Salt = salt
        }, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return userId;
    }
}