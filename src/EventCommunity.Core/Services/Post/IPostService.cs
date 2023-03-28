using EventCommunity.Core.Entities;

namespace EventCommunity.Core.Services.Post
{
    public interface IPostService
    {
        /// <summary>
        /// Get a post by its id.
        /// This also retreives author and rating information.
        /// The files can be retrieved in GetPostFiles(int postId).
        /// This is to prevent performance issues.
        /// </summary>
        /// <param name="postId">Post id</param>
        /// <returns>Post.</returns>
        Entities.Post? Get(int postId);

        /// <summary>
        /// Get files for a post.
        /// </summary>
        /// <param name="postId">Post id</param>
        /// <returns>Files for a post.</returns>
        List<PostFile> GetPostFiles(int postId);

        /// <summary>
        /// Get posts.
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Posts.</returns>
        List<Entities.Post> GetPosts(int page, int pageSize);

        /// <summary>
        /// Get posts by an author.
        /// </summary>
        /// <param name="authorId">Author id</param>
        /// <returns>Posts by an author.</returns>
        List<Entities.Post> GetPostsByAuthor(int authorId);

        /// <summary>
        /// Rate a post.
        /// </summary>
        /// <param name="postRating">Post rating</param>
        Task RatePost(PostRating postRating);

        /// <summary>
        /// Create a post.
        /// </summary>
        /// <param name="post">Post to create</param>
        /// <returns>Id of created post.</returns>
        Task<int> CreatePost(Entities.Post post);

        /// <summary>
        /// UPdate a post.
        /// </summary>
        /// <param name="post">Post to update</param>
        Task UpdatePost(Entities.Post post);

        /// <summary>
        /// Remove a post.
        /// </summary>
        /// <param name="postId">Post id</param>
        Task DeletePost(int postId);
    }
}