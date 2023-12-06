using Cifraex.Models;
using Cifraex.Services;

namespace Cifraex.Endpoints
{
    public static class CurrencyEndpoints
    {
        public static void MapCurrencyEndpoints(this WebApplication app)
        {
            app.MapGet("/api/Currency", async (HttpContext http) =>
            {
                var currencyService = http.RequestServices.GetRequiredService<CurrencyService>();
                return await currencyService.GetAllCurrenciesAsync();
            })
            .WithName("GetAllCurrencies")
            .Produces<List<Currency>>(StatusCodes.Status200OK);

            app.MapGet("/api/Currency/{code}", async (string code, HttpContext http) =>
            {
                var currencyService = http.RequestServices.GetRequiredService<CurrencyService>();
                var currency = await currencyService.GetCurrencyByCodeAsync(code);
                return currency != null ? Results.Ok(currency) : Results.NotFound();
            })
            .WithName("GetCurrencyByCode")
            .Produces<Currency>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/api/Currency", async (Currency currency, HttpContext http) =>
            {
                var currencyService = http.RequestServices.GetRequiredService<CurrencyService>();
                var createdCurrency = await currencyService.CreateCurrencyAsync(currency);
                return Results.Created($"/api/Currency/{createdCurrency.CurrencyCode}", createdCurrency);
            })
            .WithName("CreateCurrency")
            .Produces<Currency>(StatusCodes.Status201Created);

            app.MapPut("/api/Currency/{code}", async (string code, Currency updatedCurrency, HttpContext http) =>
            {
                var currencyService = http.RequestServices.GetRequiredService<CurrencyService>();
                var updated = await currencyService.UpdateCurrencyAsync(code, updatedCurrency);
                return updated ? Results.NoContent() : Results.NotFound();
            })
            .WithName("UpdateCurrency")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

            app.MapDelete("/api/Currency/{code}", async (string code, HttpContext http) =>
            {
                var currencyService = http.RequestServices.GetRequiredService<CurrencyService>();
                var deleted = await currencyService.DeleteCurrencyAsync(code);
                return deleted ? Results.Ok() : Results.NotFound();
            })
            .WithName("DeleteCurrency")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}