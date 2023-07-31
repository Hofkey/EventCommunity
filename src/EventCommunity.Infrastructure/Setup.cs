using EventCommunity.Core.Entities;
using EventCommunity.Core.Interfaces;
using EventCommunity.Infrastructure.Database;
using EventCommunity.Infrastructure.Mail;
using EventCommunity.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventCommunity.Infrastructure
{
    public static class Setup
    {
        /// <summary>
        /// Add context to a service collection.
        /// </summary>
        /// <param name="collection">The service collection</param>
        /// <param name="source">Source (server) for the database</param>
        /// <param name="name">Name of the database</param>
        /// <param name="user">Username for the database</param>
        /// <param name="password">Password for the database user</param>
        public static void AddContext(IServiceCollection collection, string source, string name, string user, string password)
        {
            collection.AddDbContext<DatabaseContext>(options =>
            {
                var connectionString = $"Data Source=.\\{source};Initial Catalog={name};Persist Security Info=True;User ID={user};Password={password};Trust Server Certificate=true";
                options.UseSqlServer(connectionString);
            });
        }

        /// <summary>
        /// Inject repositories into a service collection.
        /// </summary>
        /// <param name="collection">The service collection</param>
        public static void SetupRepositories(IServiceCollection collection)
        {
            collection.AddScoped<IRepository<Post>, GenericRepository<Post>>();
            collection.AddScoped<IRepository<PostFile>, GenericRepository<PostFile>>();
            collection.AddScoped<IRepository<User>, UserRepository>();
            collection.AddScoped<IRepository<RegisterRequest>, GenericRepository<RegisterRequest>>();
            collection.AddScoped<IRepository<CommunityEvent>, GenericRepository<CommunityEvent>>();
        }

        public static void SetupServices(IServiceCollection collection, string fromAdress, string fromPassword)
        {
            collection.AddSingleton<IMailService>(new MailService(fromAdress, "Tom & Nans Bruiloft", fromPassword));
        }
    }
}
