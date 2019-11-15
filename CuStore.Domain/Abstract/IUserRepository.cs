using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Abstract
{
    public interface IUserRepository
    {
        User GetUserById(string userId);
        UserAddress GetUserAddress(string userId);
        User GetUserByName(string userName);
        bool AddOrSaveUserAddress(UserAddress userAddress);
        int GetUsersCount();
        IEnumerable<User> GetUsers(int pageSize, int pageNumber);
        IEnumerable<User> GetUsersWithoutCrm();
        bool UpdateUsers(IEnumerable<User> users);
    }
}
