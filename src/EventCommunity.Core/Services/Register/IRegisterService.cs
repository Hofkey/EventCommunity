using EventCommunity.Core.Entities;

namespace EventCommunity.Core.Services.Register
{
    public interface IRegisterService
    {
        /// <summary>
        /// Get a register request by id.
        /// </summary>
        /// <param name="id">Id of register request</param>
        /// <returns>Register request.</returns>
        public RegisterRequest? GetRegisterRequest(int id);

        /// <summary>
        /// Get register request by token.
        /// </summary>
        /// <param name="token">Token of register request</param>
        /// <returns>Register request.</returns>
        public RegisterRequest? GetRegisterRequest(string token);

        /// <summary>
        /// Get a list of unapproved register request.
        /// </summary>
        /// <returns>List of unapproved register requests.</returns>
        public List<RegisterRequest>? GetUnapprovedRegisterRequests();

        /// <summary>
        /// Create a register request.
        /// </summary>
        /// <param name="request">Register request to create</param>
        public Task CreateRegisterRequest(RegisterRequest request);

        /// <summary>
        /// Register a user.
        /// </summary>
        /// <param name="token">Token from register request</param>
        /// <param name="password">Password for user</param>
        public Task RegisterUser(string token, string password);

        /// <summary>
        /// Approve a register request.
        /// </summary>
        /// <param name="id">Id of register request to approve</param>
        public Task ApproveRegisterRequest(int id);

        /// <summary>
        /// Remove a register request.
        /// This is also used when a register request is denied.
        /// </summary>
        /// <param name="id">Id of register request</param>
        /// <returns></returns>
        public Task DeleteRegisterRequest(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void RemindRegisterRequests();
    }
}
