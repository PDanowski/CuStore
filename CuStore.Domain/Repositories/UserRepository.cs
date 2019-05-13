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
    }
}
