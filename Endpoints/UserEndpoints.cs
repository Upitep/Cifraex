using Cifraex.Models;
using Cifraex.Services;

namespace Cifraex.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapGet("/api/User", async (HttpContext http) =>
            {
                var userService = http.RequestServices.GetRequiredService<UserService>();
                return await userService.GetAllUsersAsync();
            })
                .WithName("GetAllUsers")
                .Produces<List<User>>(StatusCodes.Status200OK);

            app.MapGet("/api/User/{id}", async (int id, HttpContext http) =>
            {
                var userService = http.RequestServices.GetRequiredService<UserService>();
                var user = await userService.GetUserByIdAsync(id);
                return user != null ? Results.Ok(user) : Results.NotFound();
            })
                .WithName("GetUserById")
                .Produces<User>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/api/User", async (User user, HttpContext http) =>
            {
                var userService = http.RequestServices.GetRequiredService<UserService>();
                var createdUser = await userService.CreateUserAsync(user);
                return Results.Created($"/api/User/{createdUser.UserId}", createdUser);
            })
                .WithName("CreateUser")
                .Produces<User>(StatusCodes.Status201Created);

            app.MapPut("/api/User/{id}", async (int id, User updateUser, HttpContext http) =>
            {
                var userService = http.RequestServices.GetRequiredService<UserService>();
                var updated = await userService.UpdateUserAsync(id, updateUser);
                return updated ? Results.NoContent() : Results.NotFound();
            })
                .WithName("UpdateUser")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);

            app.MapDelete("/api/User/{id}", async (int id, HttpContext http) =>
            {
                var userService = http.RequestServices.GetRequiredService<UserService>();
                var deleted = await userService.DeleteUserAsync(id);
                return deleted ? Results.Ok() : Results.NotFound();
            })
                .WithName("DeleteUser")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            app.MapPut("/api/User/{id}/balance", async (int id, UserBalanceUpdateRequest balanceUpdate, HttpContext http) =>
            {
                var userService = http.RequestServices.GetRequiredService<UserService>();
                var updated = await userService.UpdateUserBalanceAsync(id, balanceUpdate.Currency, balanceUpdate.Amount);
                return updated ? Results.Ok() : Results.NotFound();
            })
                .WithName("UpdateUserBalance")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);
        }
    }

    public class UserBalanceUpdateRequest
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
