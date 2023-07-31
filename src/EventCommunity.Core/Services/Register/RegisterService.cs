using EventCommunity.Core.Entities;
using EventCommunity.Core.Exceptions;
using EventCommunity.Core.Interfaces;
using EventCommunity.Shared.Security.Encryption;

namespace EventCommunity.Core.Services.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly IMailService mailService;
        private readonly IRepository<RegisterRequest> registerRequestRepository;
        private readonly IRepository<Entities.User> userRespository;

        public RegisterService(IMailService mailService,
            IRepository<RegisterRequest> registerRequestRepository,
            IRepository<Entities.User> userRespository)
        {
            this.mailService = mailService;
            this.registerRequestRepository = registerRequestRepository;
            this.userRespository = userRespository;
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

            string content = $"Jouw verzoek is goedgekeurd! u kunt uw token ({request.Token}) gebruikern op: http://bruiloft.omnitudo.space/auth (de finalize tab.)!";
            mailService.Send(content, "Token aangemaakt.", request.Email);
        }

        public async Task DeleteRegisterRequest(int id)
        {
            await registerRequestRepository.Delete(id);
        }


        public void RemindRegisterRequests()
        {
            var requests = registerRequestRepository.Get(r => r.Id != 0);

            foreach (var request in requests)
            {
                string content = $"Jouw token is nog niet gebruikt. Rond jouw registratie af! Token: {request.Token}";
                mailService.Send(content, "Token aangemaakt.", request.Email);
            }
        }

        public async Task RegisterUser(string token, string password)
        {
            var request = registerRequestRepository.Get(r => r.Token == token).SingleOrDefault();

            if (request == null)
            {
                throw new EntityDoesNotExistException(typeof(RegisterRequest), token);
            }

            await userRespository.Create(new Entities.User
            {
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Password = PasswordHelper.GetHash(password)
            });

            await DeleteRegisterRequest(request.Id);
        }
    }
}
