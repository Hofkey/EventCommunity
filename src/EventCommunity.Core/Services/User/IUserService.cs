using EventCommunity.Core.Enums;

namespace EventCommunity.Core.Services.User
{
    public interface IUserService
    {
        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="user">User to add</param>
        /// <returns>Id of the new user.</returns>
        Task<int> AddUser(Entities.User user);

        /// <summary>
        /// Change a password for a user.
        /// </summary>
        /// <param name="id">Id for the user</param>
        /// <param name="oldPassword">Old password for user</param>
        /// <param name="newPassword">New password for user</param>
        /// <returns>True if succeeded; false if not.</returns>
        Task<bool> ChangeUserPassword(int id, string oldPassword, string newPassword);

        /// <summary>
        /// Change the role for a user.
        /// </summary>
        /// <param name="id">Id for the user</param>
        /// <param name="role">Role for the user</param>
        Task ChangeUserRole(int id, UserRole role);

        /// <summary>
        /// Get a user by its Id.
        /// </summary>
        /// <param name="id">Id for user</param>
        /// <returns>Found user.</returns>
        Entities.User GetUser(int id);
    }
}