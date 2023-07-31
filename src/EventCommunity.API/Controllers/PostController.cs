using AutoMapper;
using EventCommunity.API.Models.Dto;
using EventCommunity.Core.Entities;
using EventCommunity.Core.Services.Post;
using EventCommunity.Core.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventCommunity.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    [Authorize(Roles = "Admin,Guest")]
    public class PostController : ControllerBase
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IPostService postService;
        private readonly IUserService userService;

        public PostController(IWebHostEnvironment hostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IPostService postService,
            IUserService userService)
        {
            this.hostEnvironment = hostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.postService = postService;
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult<PostResponseDto> Get(int page, int size)
        {
            var posts = postService.GetPosts(page, size);
            var mapped = mapper.Map<List<PostDto>>(posts);

            foreach (var post in mapped)
            {
                var files = postService.GetPostFiles(post.Id).Select(x => x.Path);

                foreach (var file in files)
                {
                    post.Files.Add($"{httpContextAccessor.HttpContext.Request.Scheme}" +
                        $"://{httpContextAccessor.HttpContext.Request.Host}" +
                        $"/api/uploads/{Path.GetFileName(file)}");
                }
            }

            return Ok(new PostResponseDto
            {
                Posts = mapped,
                Count = postService.GetPostCount()
            });
        }

        [HttpGet("{id}")]
        public ActionResult<List<PostDto>> Get(int id)
        {
            var post = postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            var result = mapper.Map<PostDto>(post);

            foreach (var file in post.Files)
            {
                result.Files.Add(file.Path);
            }

            return Ok(result);
        }

        [HttpGet("author/{id}")]
        public ActionResult<List<PostDto>> GetByAuthor(int author)
        {
            var posts = postService.GetPostsByAuthor(author);

            if (posts == null)
            {
                return NotFound();
            }

            var result = new List<PostDto>();

            foreach (var post in posts)
            {
                var mapped = mapper.Map<PostDto>(post);

                foreach (var file in post.Files)
                {
                    mapped.Files.Add(file.Path);
                }

                result.Add(mapped);
            }

            return Ok(result);
        }

        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Post([FromForm] PostUploadDto post)
        {
            var author = userService.GetUser(post.Author);

            var newPost = new Post
            {
                AuthorId = author.Id,
                Description = post.Description,
                Posted = post.Posted,
                RatingNegative = 0,
                RatingPositive = 0,
                Title = post.Title,
            };

            await postService.CreatePost(newPost);

            try
            {

                foreach (var file in post.Files)
                {
                    var fileName = $"{newPost.Id}_{file.FileName}";
                    var filePath = Path.Combine("C:\\inetpub\\omnitudo\\api\\wwwroot\\uploads", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var uploadDate = DateTime.Now;

                    await postService.AddPostFile(new PostFile
                    {
                        PostId = newPost.Id,
                        Path = filePath,
                        Name = fileName,
                        Uploaded = uploadDate
                    });
                }

                return Ok(newPost.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, innerMessage = ex.InnerException?.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] PostUploadDto post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapped = mapper.Map<Post>(post);

            await postService.UpdatePost(mapped);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (postService.Get(id) == null)
            {
                return NotFound();
            }

            await postService.DeletePost(id);

            return Ok();
        }
    }
}
