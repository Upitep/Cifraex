using Cifraex.Services;

namespace Cifraex.Endpoints
{
    public static class ExchangeEndpoints
    {
        public static void MapExchangeEndpoints(this WebApplication app)
        {
            app.MapPost("/api/Exchange", async (ExchangeRequest request, HttpContext http) =>
                {
                    try
                    {
                        var exchangeService = http.RequestServices.GetRequiredService<ExchangeService>();

                        await exchangeService.ExchangeCurrencyAsync(
                            request.UserId,
                            request.FromCurrency,
                            request.ToCurrency,
                            request.Amount,
                            request.ExchangeRate,
                            request.CommissionRate);

                        return Results.Ok("Обмен валюты выполнен успешно.");
                    }
                    catch (Exception ex)
                    {
                        return Results.BadRequest(ex.Message);
                    }
                })
                .WithName("ExchangeCurrency")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }


    public class ExchangeRequest
    {
        public int UserId { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal CommissionRate { get; set; } = 0.0005m;
    }
}
