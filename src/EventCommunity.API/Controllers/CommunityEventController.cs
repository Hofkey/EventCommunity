using AutoMapper;
using EventCommunity.API.Models.Dto;
using EventCommunity.Core.Entities;
using EventCommunity.Core.Services.CommunityEvent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventCommunity.API.Controllers
{
    [Route("api/events")]
    [ApiController]
    [Authorize(Roles = "Admin,Guest")]
    public class CommunityEventController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICommunityEventService communityEventService;

        public CommunityEventController(IMapper mapper,
            ICommunityEventService communityEventService)
        {
            this.mapper = mapper;
            this.communityEventService = communityEventService;
        }

        [HttpGet("{id}")]
        public ActionResult<CommunityEventDto> Get(int id)
        {
            try
            {
                var communityEvent = communityEventService.Get(id);

                if (communityEvent == null)
                {
                    return NotFound("Event not found.");
                }

                return Ok(mapper.Map<CommunityEventDto>(communityEvent));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error has occured while retrieving the event.");
            }
        }

        [HttpGet]
        public ActionResult<List<CommunityEventDto>> GetEventsFor()
        {
            try
            {
                var communityEvents = communityEventService.GetCommunityEvents();

                if (!communityEvents.Any())
                {
                    return NotFound("Events not found for given date(s).");
                }

                return Ok(communityEvents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error has occured while retrieving the events.");
            }
        }

        [HttpGet("date")]
        public ActionResult<List<CommunityEventDto>> GetEventsForDate(DateTime start, DateTime? end)
        {
            try
            {
                end ??= start;

                var communityEvents = communityEventService.GetCommunityEventsByDate(start, end);

                if (!communityEvents.Any())
                {
                    return NotFound("Events not found for given date(s).");
                }

                return Ok(communityEvents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error has occured while retrieving the events.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CommunityEventDto communityEventDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var communityEvent = mapper.Map<CommunityEvent>(communityEventDto);
                await communityEventService.AddEvent(communityEvent);

                return Ok(communityEvent.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error has occured while creating the event.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] CommunityEventDto communityEventDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await communityEventService.EditEvent(mapper.Map<CommunityEvent>(communityEventDto));

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error has occured while editing the event.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await communityEventService.DeleteEvent(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error has occured while deleting the event.");
            }
        }
    }
}
