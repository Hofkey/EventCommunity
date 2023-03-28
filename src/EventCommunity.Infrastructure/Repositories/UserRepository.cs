using EventCommunity.Core.Entities;
using EventCommunity.Core.Exceptions;
using EventCommunity.Core.Interfaces;
using EventCommunity.Infrastructure.Database;
using EventCommunity.Shared.Security.Encryption;

namespace EventCommunity.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }

        public override async Task<int> Create(User entity)
        {
            if (Get(x => x.Email == entity.Email).Any())
            {
                throw new DuplicateEntityException("User", "Username", "User already exists!");
            }

            entity.Password = PasswordHelper.GetHash(entity.Password);

            return await base.Create(entity);
        }

        public User? ValidatePassword(string email, string password)
        {
            var user = Get(x => x.Email == email).SingleOrDefault();

            if (user == null)
            {
                return null;
            }

            PasswordHelper.IsValid(password, user.Password);

            return user;
        }
    }
}
