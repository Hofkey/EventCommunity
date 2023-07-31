using AutoMapper;
using EventCommunity.API.Models;
using EventCommunity.API.Models.Dto;
using EventCommunity.Core.Entities;
using EventCommunity.Core.Enums;
using EventCommunity.Core.Exceptions;
using EventCommunity.Core.Services.User;
using EventCommunity.Shared.Security.Encryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventCommunity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public UserController(IMapper mapper,
            IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult<List<UserDto>> Get()
        {
            return Ok(mapper.Map<List<UserDto>>(userService.GetUsers()));
        }

        // GET: api/<UserController>
        [HttpGet("{id}")]
        public ActionResult<List<User>> GetUser(int id)
        {
            try
            {
                return Ok(userService.GetUser(id));
            }
            catch (EntityDoesNotExistException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateUser([FromBody] NewUserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = mapper.Map<User>(newUser);

            user.Password = PasswordHelper.GetHash(newUser.Password);

            try
            {
                return Ok(await userService.AddUser(user));
            }
            catch (DuplicateEntityException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/role")]
        public async Task<ActionResult> UpdateUserRole(int id, [FromBody] UserRole userRole)
        {
            try
            {
                await userService.ChangeUserRole(id, userRole);
                return Ok();
            }
            catch (EntityDoesNotExistException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/password")]
        public async Task<ActionResult> ChangeUserPassword(int id, [FromBody] PasswordChange passwordChange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await userService.ChangeUserPassword(id,
                    passwordChange.OldPassword,
                    passwordChange.NewPassword);

                return Ok(result);
            }
            catch (EntityDoesNotExistException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
