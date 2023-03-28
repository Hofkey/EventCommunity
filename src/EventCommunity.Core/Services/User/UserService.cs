using EventCommunity.Core.Enums;
using EventCommunity.Core.Interfaces;
using EventCommunity.Shared.Security.Encryption;

namespace EventCommunity.Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRepository<Entities.User> userRepository;

        public UserService(IRepository<Entities.User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public Entities.User GetUser(int id)
        {
            return userRepository.GetById(id);
        }

        public async Task<int> AddUser(Entities.User user)
        {
            return await userRepository.Create(user);
        }

        public async Task ChangeUserRole(int id, UserRole role)
        {
            var user = GetUser(id);
            user.UserRole = role;
            await userRepository.Update(user);
        }

        public async Task<bool> ChangeUserPassword(int id, string oldPassword, string newPassword)
        {
            var user = GetUser(id);

            if (!PasswordHelper.IsValid(oldPassword, user.Password))
            {
                return false;
            }

            user.Password = PasswordHelper.GetHash(newPassword);

            await userRepository.Update(user);

            return true;
        }
    }
}
