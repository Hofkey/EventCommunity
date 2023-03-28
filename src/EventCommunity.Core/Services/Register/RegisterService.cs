using EventCommunity.Core.Entities;
using EventCommunity.Core.Exceptions;
using EventCommunity.Core.Interfaces;

namespace EventCommunity.Core.Services.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly IMailService mailService;
        private readonly IRepository<RegisterRequest> registerRequestRepository;

        public RegisterService(IMailService mailService, 
            IRepository<RegisterRequest> registerRequestRepository)
        {
            this.mailService = mailService;
            this.registerRequestRepository = registerRequestRepository;
        }

        public RegisterRequest? GetRegisterRequest(int id)
        {
            return registerRequestRepository.GetById(id);
        }

        public RegisterRequest? GetRegisterRequest(string token)
        {
            return registerRequestRepository.Get(request => request.Token == token).SingleOrDefault();
        }

        public List<RegisterRequest>? GetUnapprovedRegisterRequests()
        {
            return registerRequestRepository.Get(request => !request.Approved).ToList();
        }

        public async Task CreateRegisterRequest(RegisterRequest request)
        {
            if (registerRequestRepository.Get(x => x.Email == request.Email).Any())
            {
                throw new DuplicateEntityException("TokenUser", "Email", request.Email);
            }

            string code = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            code = code.Replace("=", "");
            code = code.Replace("+", "");

            request.Token = code;

            await registerRequestRepository.Create(request);

            string content = "Jouw verzoek is verstuurd! Wij keuren het binnenkort goed, dus houd je mail in de gaten (ook de spam folder)!";
            mailService.Send(content, "Token aangemaakt.", request.Email);
        }

        public async Task ApproveRegisterRequest(int id)
        {
            var request = registerRequestRepository.Get(r => r.Id == id).SingleOrDefault();

            if (request == null)
            {
                throw new EntityDoesNotExistException(typeof(RegisterRequest), id);
            }

            request.Approved = true;

            await registerRequestRepository.Update(request);

            string content = "Jouw verzoek is goedgekeurd! Wij keuren het binnenkort goed, dus houd je mail in de gaten (ook de spam folder)!";
            mailService.Send(content, "Token aangemaakt.", request.Email);
        }

        public Task DeleteRegisterRequest(int id)
        {
            throw new NotImplementedException();
        }


        public Task RemindRegisterRequests()
        {
            throw new NotImplementedException();
        }
    }
}
