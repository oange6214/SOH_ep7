using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SohatNotebook.DataService.Data;
using SohatNotebook.DataService.IRepository;
using SohatNotebook.Entities.DbSet;

namespace SohatNotebook.DataService.Repository;

public class UsersRepository : GenericRepository<User>, IUsersRepository
{
    public UsersRepository(AppDbContext context, ILogger logger) : 
        base(context, logger)
    {
    }

    public override async Task<IEnumerable<User>> All()
    {
        try
        {
            return await _dbSet.Where(x => x.Status == 1)
                               .AsNoTracking()
                               .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} All method has generated an error", typeof(UsersRepository));
            return new List<User>();
        }
    }

    public async Task<bool> UpdateUserProfile(User user)
    {
        try
        {
            var exitingUser =  await _dbSet.Where(x => x.Status == 1 && x.Id == user.Id)
                                                .FirstOrDefaultAsync();

            if (exitingUser == null)
                return false;

            exitingUser.FirstName = user.FirstName;
            exitingUser.LastName = user.LastName;
            exitingUser.MobileNumber = user.MobileNumber;
            exitingUser.Phone = user.Phone;
            exitingUser.Sex = user.Sex;
            exitingUser.Address = user.Address;
            exitingUser.UpdateDate = DateTime.UtcNow;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} UpdateUserProfile method has generated an error", typeof(UsersRepository));
            return false;
        }
    }

    public async Task<User> GetByIdentityId(Guid identityId)
    {
        try
        {
            return await _dbSet.Where(x => x.Status == 1 && x.IdentityId == identityId)
                               .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} GetByIdentityId method has generated an error", typeof(UsersRepository));
            return null;
        }
    }
}