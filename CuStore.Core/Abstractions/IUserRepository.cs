using CuStore.Core.Entities;

namespace CuStore.Core.Abstractions;

public interface IUserRepository
{
    User? GetUserById(string userId);
    UserAddress? GetUserAddress(string userId);
    User? GetUserByName(string userName);
    bool AddUser(User user);
    bool AddOrSaveUserAddress(UserAddress userAddress);
    int GetUsersCount();
    IEnumerable<User> GetUsers(int pageSize, int pageNumber);
    IEnumerable<User> GetUsersWithoutCrm();
    bool UpdateUsers(IEnumerable<User> users);
}
