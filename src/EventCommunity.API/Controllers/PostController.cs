using AutoMapper;
using EventCommunity.API.Models;
using EventCommunity.Core.Entities;
using EventCommunity.Core.Services.Post;
using EventCommunity.Core.Services.User;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventCommunity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPostService postService;
        private readonly IUserService userService;

        public PostController(IMapper mapper,
            IPostService postService, 
            IUserService userService)
        {
            this.mapper = mapper;
            this.postService = postService;
            this.userService = userService;
        }

        // GET: api/<PostController>
        [HttpGet]
        public IActionResult Get(int page, int size)
        {
            var posts = postService.GetPosts(page, size);
            var mapped = mapper.Map<List<PostPOCO>>(posts);
            return Ok(mapped);
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = postService.Get(id);

            if(post == null)
            {
                return NotFound();
            }

            var mapped = mapper.Map<PostPOCO>(post);

            mapped.RatingPositive = post.Ratings.Count(rating => rating.Positive);
            mapped.RatingNegative = post.Ratings.Count(rating => !rating.Positive);

            return Ok(mapped);
        }

        [HttpGet("author/{id}")]
        public IActionResult GetByAuthor(int author)
        {
            var posts = postService.GetPostsByAuthor(author);

            if(posts == null)
            {
                return NotFound();
            }

            var mapped = mapper.Map<List<PostPOCO>>(posts);
            return Ok(mapped);
        }

        // POST api/<PostController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostPOCO post)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapped = mapper.Map<Post>(post);
            var result = await postService.CreatePost(mapped);

            return Ok(result);
        }

        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] PostPOCO post)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapped = mapper.Map<Post>(post);

            await postService.UpdatePost(mapped);

            return Ok();
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(postService.Get(id) == null)
            {
                return NotFound();
            }

            await postService.DeletePost(id);

            return Ok();
        }
    }
}
