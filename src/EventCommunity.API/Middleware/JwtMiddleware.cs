using EventCommunity.Core.Services.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EventCommunity.API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate RequestDelegate;
        private readonly IConfiguration Configiration;

        public JwtMiddleware(RequestDelegate next,
            IConfiguration config)
        {
            RequestDelegate = next;
            Configiration = config;
        }

        public async Task InvokeAsync(HttpContext context, IUserService UserService)
        {
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Configiration["Jwt:Secret"]);
                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out var validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                    var user = UserService.GetUser(userId);

                    if (user != null)
                    {
                        context.Items["User"] = user;

                        var role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
                        if (!string.IsNullOrEmpty(role))
                        {
                            context.Items["UserRole"] = role;
                        }
                    }
                }
                catch
                {
                    // Do nothing if the token validation fails.
                }
            }

            await RequestDelegate(context);
        }
    }
}
