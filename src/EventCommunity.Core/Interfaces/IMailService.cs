namespace EventCommunity.Core.Interfaces
{
    public interface IMailService
    {
        /// <summary>
        /// Send an email.
        /// </summary>
        /// <param name="content">Contents</param>
        /// <param name="subj">Subject</param>
        /// <param name="toAdress">Recipient adress</param>
        /// <param name="toName">Recipient name</param>
        void Send(string content, string subj, string toAdress, string? toName = null);
    }
}
