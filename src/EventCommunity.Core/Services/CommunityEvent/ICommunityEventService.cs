namespace EventCommunity.Core.Services.CommunityEvent
{
    public interface ICommunityEventService
    {
        /// <summary>
        /// Gets an event by Id.
        /// </summary>
        /// <param name="communityEventId">Id of the event.</param>
        /// <returns>Found event.</returns>
        Entities.CommunityEvent? Get(int communityEventId);

        /// <summary>
        /// Gets the current countdown event date.
        /// </summary>
        /// <returns>The current countdown event date.</returns>
        DateTime GetCountDownEventDate();

        /// <summary>
        /// Gets all the community events.
        /// </summary>
        /// <returns>List of all community events.</returns>
        List<Entities.CommunityEvent> GetCommunityEvents();

        /// <summary>
        /// Gets all community events based on dates.
        /// </summary>
        /// <param name="date">Start or single date.</param>
        /// <param name="end">Optional end date.</param>
        /// <returns>List of community events.</returns>
        List<Entities.CommunityEvent> GetCommunityEventsByDate(DateTime date, DateTime? end = null);

        /// <summary>
        /// Adds an event.
        /// </summary>
        /// <param name="communityEvent">Event to add.</param>
        /// <returns>Id of created event.</returns>
        Task<int> AddEvent(Entities.CommunityEvent communityEvent);

        /// <summary>
        /// Edits an event.
        /// </summary>
        /// <param name="communityEvent">Event to edit.</param>
        Task EditEvent(Entities.CommunityEvent communityEvent);

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="id">Id of event to delete.</param>
        Task DeleteEvent(int id);
    }
}