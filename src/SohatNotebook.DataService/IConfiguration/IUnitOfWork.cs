using SohatNotebook.DataService.IRepository;

namespace SohatNotebook.DataService.IConfiguration;

public interface IUnitOfWork
{
    IUsersRepository Users { get; }
    IRefreshTokensRepository RefreshTokens { get; }
    IHealthDatasRepository HealthDatas { get; }

    Task CompleteAsync();
}