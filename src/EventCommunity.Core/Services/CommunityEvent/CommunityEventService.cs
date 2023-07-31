using EventCommunity.Core.Exceptions;
using EventCommunity.Core.Interfaces;

namespace EventCommunity.Core.Services.CommunityEvent
{
    public class CommunityEventService : ICommunityEventService
    {
        private readonly IRepository<Entities.CommunityEvent> communityEventRepository;

        public CommunityEventService(IRepository<Entities.CommunityEvent> communityEventRepository)
        {
            this.communityEventRepository = communityEventRepository;
        }

        public Entities.CommunityEvent? Get(int communityEventId)
        {
            return communityEventRepository.GetById(communityEventId);
        }

        public List<Entities.CommunityEvent> GetCommunityEvents()
        {
            return communityEventRepository.Get(communityEvent => communityEvent.Id > 0).ToList();
        }

        public DateTime GetCountDownEventDate()
        {
            var countDownEvent = communityEventRepository.Get(communityEvent => communityEvent.IsCountdownEvent).FirstOrDefault();

            if (countDownEvent == null)
            {
                throw new Exception("There is no countdown event.");
            }

            return countDownEvent!.Start;
        }

        public List<Entities.CommunityEvent> GetCommunityEventsByDate(DateTime date, DateTime? end = null)
        {
            return end != null
                ? communityEventRepository.Get(communityEvent => communityEvent.Start >= date
                    && communityEvent.End <= end.Value).ToList()
                : communityEventRepository.Get(communityEvent => communityEvent.Start >= date
                && communityEvent.End <= date).ToList();
        }

        public async Task<int> AddEvent(Entities.CommunityEvent communityEvent)
        {
            return await communityEventRepository.Create(communityEvent);
        }

        public async Task EditEvent(Entities.CommunityEvent communityEvent)
        {
            if (!communityEventRepository.Get(ce => ce.Id == communityEvent.Id).Any())
            {
                throw new EntityDoesNotExistException(typeof(Entities.CommunityEvent), communityEvent.Id);
            }

            await communityEventRepository.Update(communityEvent);
        }

        public async Task DeleteEvent(int id)
        {
            if (!communityEventRepository.Get(communityEvent => communityEvent.Id == id).Any())
            {
                throw new EntityDoesNotExistException(typeof(Entities.CommunityEvent), id);
            }

            await communityEventRepository.Delete(id);
        }
    }
}
