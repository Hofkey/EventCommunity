using EventCommunity.Core.Entities;
using EventCommunity.Core.Exceptions;
using EventCommunity.Core.Interfaces;

namespace EventCommunity.Core.Services.Post
{
    public class PostService : IPostService
    {
        private readonly IRepository<Entities.Post> postRepository;
        private readonly IRepository<PostFile> postFileRepository;

        /// <summary>
        /// Post service,
        /// this service contains all the logic regarding posts.
        /// </summary>
        /// <param name="postRepository">The post repository</param>
        /// <param name="postRatingRepository">The post rating repository</param>
        /// <param name="postFileRepository">The post file repository</param>
        public PostService(IRepository<Entities.Post> postRepository,
            IRepository<PostFile> postFileRepository)
        {
            this.postRepository = postRepository;
            this.postFileRepository = postFileRepository;
        }

        public Entities.Post? Get(int postId)
        {
            return postRepository.Get(post => post.Id == postId, null, "Author").SingleOrDefault();
        }

        public List<PostFile> GetPostFiles(int postId)
        {
            return postFileRepository.Get(postFile => postFile.PostId == postId).ToList();
        }

        public List<Entities.Post> GetPosts(int page, int pageSize)
        {
            return postRepository.Get(post => post.Id > 0, null, "Author")
                .Skip(page)
                .Take(pageSize)
                .ToList();
        }

        public List<Entities.Post> GetPostsByAuthor(int authorId)
        {
            return postRepository.Get(post => post.AuthorId == authorId).ToList();
        }

        public async Task<int> CreatePost(Entities.Post post)
        {
            return await postRepository.Create(post);
        }

        public int GetPostCount()
        {
            return postRepository.Get(posts => posts.Id > 0).Count();
        }

        public async Task AddPostFile(PostFile postFile)
        {
            await postFileRepository.Create(postFile);
        }

        public async Task UpdatePost(Entities.Post post)
        {
            if (!postRepository.Get(_post => _post.Id == post.Id).Any())
            {
                throw new EntityDoesNotExistException(typeof(Entities.Post), post.Id);
            }

            await postRepository.Update(post);
        }

        public async Task DeletePost(int postId)
        {
            var result = postRepository.Get(post => post.Id == postId, null, "Files, Ratings")
                .SingleOrDefault();

            if (result == null)
            {
                throw new EntityDoesNotExistException(typeof(Entities.Post), postId);
            }

            foreach (var file in result.Files)
            {
                await postFileRepository.Delete(file.Id);
            }

            await postRepository.Delete(postId);
        }
    }
}
