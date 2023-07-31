using EventCommunity.API.Middleware;
using EventCommunity.Core.Services.CommunityEvent;
using EventCommunity.Core.Services.Post;
using EventCommunity.Core.Services.Register;
using EventCommunity.Core.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(builder.Configuration.GetValue<string>("Jwt:Secret"))),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = null;
});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
});

// Setup infrastructure.
#region Infrastructure setup.
EventCommunity.Infrastructure.Setup.AddContext(builder.Services,
    builder.Configuration.GetValue<string>("Database:Source"),
    builder.Configuration.GetValue<string>("Database:Name"),
    builder.Configuration.GetValue<string>("Database:User"),
    builder.Configuration.GetValue<string>("Database:Password"));

EventCommunity.Infrastructure.Setup.SetupRepositories(builder.Services);
EventCommunity.Infrastructure.Setup.SetupServices(builder.Services,
    builder.Configuration.GetValue<string>("Mailing:Email"),
    builder.Configuration.GetValue<string>("Mailing:Password"));
#endregion

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<ICommunityEventService, CommunityEventService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseHsts();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "uploads")),
    RequestPath = "/uploads"
});

app.UseCors();

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
