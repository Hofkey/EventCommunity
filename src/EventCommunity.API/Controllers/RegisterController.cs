using AutoMapper;
using EventCommunity.API.Models;
using EventCommunity.API.Models.Dto;
using EventCommunity.Core.Entities;
using EventCommunity.Core.Exceptions;
using EventCommunity.Core.Services.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventCommunity.API.Controllers
{
    [Route("api/register")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RegisterController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRegisterService registerService;

        public RegisterController(IMapper mapper,
            IRegisterService registerService)
        {
            this.mapper = mapper;
            this.registerService = registerService;
        }

        [HttpGet("unapproved")]
        public ActionResult<List<RegisterRequestDto>> GetUnapprovedRegisterRequests()
        {
            var requests = registerService.GetUnapprovedRegisterRequests();

            return Ok(mapper.Map<List<RegisterRequestDto>>(requests));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateRegisterRequest([FromBody] NewRegisterRequestDto registerRequestPOCO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerRequest = new RegisterRequest
            {
                Firstname = registerRequestPOCO.Firstname,
                Lastname = registerRequestPOCO.Lastname,
                Email = registerRequestPOCO.Email,
                PhoneNumber = registerRequestPOCO.PhoneNumber,
                Approved = false
            };

            try
            {
                await registerService.CreateRegisterRequest(registerRequest);

                return Ok();
            }
            catch (DuplicateEntityException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("finalize")]
        [AllowAnonymous]
        public async Task<ActionResult> FinalizeRegistration([FromBody] RegistrationFinalization registrationFinalization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await registerService.RegisterUser(registrationFinalization.Token, registrationFinalization.Password);

                return Ok();
            }
            catch (EntityDoesNotExistException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("sendreminders")]
        public ActionResult SendReminders()
        {
            registerService.RemindRegisterRequests();

            return Ok();
        }

        [HttpPut("{id}/approve")]
        public async Task<ActionResult> ApproveRegisterRequest(int id)
        {
            try
            {
                await registerService.ApproveRegisterRequest(id);

                return Ok();
            }
            catch (EntityDoesNotExistException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRegisterRequest(int id)
        {
            try
            {
                await registerService.DeleteRegisterRequest(id);

                return Ok();
            }
            catch (EntityDoesNotExistException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
