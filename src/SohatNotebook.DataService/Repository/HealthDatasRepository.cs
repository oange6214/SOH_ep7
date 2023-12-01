using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SohatNotebook.DataService.Data;
using SohatNotebook.DataService.IRepository;
using SohatNotebook.Entities.DbSet;

namespace SohatNotebook.DataService.Repository;

public class HealthDatasRepository : GenericRepository<HealthData>, IHealthDatasRepository
{
    public HealthDatasRepository(AppDbContext context, ILogger logger) : 
        base(context, logger)
    {
    }

    public override async Task<IEnumerable<HealthData>> All()
    {
        try
        {
            return await _dbSet.Where(x => x.Status == 1)
                               .AsNoTracking()
                               .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} All method has generated an error", typeof(HealthDatasRepository));
            return new List<HealthData>();
        }
    }

    public async Task<bool> UpdateHealthDataProfile(HealthData healthData)
    {
        try
        {
            var exitingHealthData =  await _dbSet.Where(x => x.Status == 1 && x.Id == healthData.Id)
                                                .FirstOrDefaultAsync();

            if (exitingHealthData == null)
                return false;

            exitingHealthData.BloodType = healthData.BloodType;
            exitingHealthData.Height = healthData.Height;
            exitingHealthData.Weight = healthData.Weight;
            exitingHealthData.Race = healthData.Race;
            exitingHealthData.BloodType = healthData.BloodType;
            exitingHealthData.UpdateDate = DateTime.UtcNow;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} UpdateHealthDataProfile method has generated an error", typeof(HealthDatasRepository));
            return false;
        }
    }
}