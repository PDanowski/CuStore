using CuStore.Core.Abstractions;
using CuStore.Core.Entities;
using CuStore.Infrastructure.Data;

namespace CuStore.Infrastructure.Repositories;

public class UserRepository(StoreDbContext context) : IUserRepository
{
    public User? GetUserById(string userId) =>
        context.Users.FirstOrDefault(u => u.Id == userId);

    public UserAddress? GetUserAddress(string userId) =>
        context.UserAddresses.FirstOrDefault(u => u.UserId == userId);

    public User? GetUserByName(string userName) =>
        context.Users.FirstOrDefault(u => u.UserName == userName);

    public bool AddUser(User user)
    {
        if (context.Users.Any(u => u.Id == user.Id))
        {
            return false;
        }

        context.Users.Add(user);
        context.SaveChanges();
        return true;
    }

    public bool AddOrSaveUserAddress(UserAddress userAddress)
    {
        if (userAddress.Id == default)
        {
            context.UserAddresses.Add(userAddress);
        }
        else
        {
            var existingUserAddress = context.UserAddresses
                .SingleOrDefault(u => u.Id == userAddress.Id);
            if (existingUserAddress is null)
            {
                return false;
            }

            context.Entry(existingUserAddress).CurrentValues.SetValues(userAddress);
        }

        context.SaveChanges();
        return true;
    }

    public int GetUsersCount() => context.Users.Count();

    public IEnumerable<User> GetUsers(int pageSize, int pageNumber) =>
        context.Users.OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

    public IEnumerable<User> GetUsersWithoutCrm() =>
        context.Users.Where(u => !u.CrmGuid.HasValue || u.CrmGuid == Guid.Empty).ToList();

    public bool UpdateUsers(IEnumerable<User> users)
    {
        foreach (var user in users)
        {
            var existingUser = context.Users.SingleOrDefault(u => u.Id == user.Id);
            if (existingUser is null)
            {
                continue;
            }

            context.Entry(existingUser).CurrentValues.SetValues(user);
        }

        context.SaveChanges();
        return true;
    }
}
