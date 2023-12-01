
using SohatNotebook.Entities.DbSet;

namespace SohatNotebook.DataService.IRepository;
public interface IHealthDatasRepository : IGenericRepository<HealthData>
{
    Task<bool> UpdateHealthDataProfile(HealthData user);
}