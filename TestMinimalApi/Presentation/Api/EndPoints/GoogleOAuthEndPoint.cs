using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TestMinimalApi.Presentation.Api.EndPoints
{
    public static class GoogleOAuthEndPoint
    {
        public static void MapGoogleOAuthEndPoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/tesminimalapi").
                       //RequireAuthorization().
                       WithTags("Google OAuth2");
            group.MapGet("/login/google", async (HttpContext context) =>
            {
                await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
          new AuthenticationProperties
          {
              RedirectUri = "api/tesminimalapi/profile"
          });
            });

            group.MapGet("/profile", (HttpContext context) =>
            {
                var user = context.User;

                return Results.Ok(new
                {
                    Name = user.Identity?.Name,
                    Email = user.Claims.FirstOrDefault(c => c.Type.Contains("email"))?.Value
                }
               );
            })
  .RequireAuthorization();
            group.MapGet("/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync();
                return Results.Ok("Logged out");
            });

        }
    }
}
