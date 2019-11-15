using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IStoreContext _context;

        public UserRepository(IStoreContext context)
        {
            _context = context;
        }

        public UserAddress GetUserAddress(string userId)
        {
            return _context.UserAddresses.FirstOrDefault(u => u.UserId.Equals(userId));
        }

        public bool AddOrSaveUserAddress(UserAddress userAddress)
        {
            try
            {
                if (userAddress.Id == default(int))
                {
                    userAddress.User = GetUserById(userAddress.UserId);
                    _context.UserAddresses.Add(userAddress);
                }
                else
                {
                    var existingUserAddress = _context.UserAddresses
                        .SingleOrDefault(u => u.Id == userAddress.Id);

                    // Update 
                    _context.Entry(existingUserAddress).CurrentValues.SetValues(userAddress);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public User GetUserById(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id.Equals(userId));
        }

        public User GetUserByName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName.Equals(userName));
        }

        public int GetUsersCount()
        {
            return _context.Users.Count();
        }

        public IEnumerable<User> GetUsers(int pageSize, int pageNumber)
        {
            return _context.Users.OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<User> GetUsersWithoutCrm()
        {
            return _context.Users.Where(u => !u.CrmGuid.HasValue);
        }

        public bool UpdateUsers(IEnumerable<User> users)
        {
            try
            {
                foreach (var user in users)
                {
                    var existingUser = _context.Users
                            .SingleOrDefault(u => u.Id == user.Id);
                        // Update 
                        _context.Entry(existingUser).CurrentValues.SetValues(user);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }

        }
    }
}
