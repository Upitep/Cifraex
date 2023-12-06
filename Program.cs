using Cifraex.Data;
using Cifraex.Endpoints;
using Cifraex.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Cifraex
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.MapUserEndpoints();
            app.MapCurrencyEndpoints();
            app.MapExchangeEndpoints();

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Добавление контекста базы данных
            services.AddDbContext<ExchangeContext>(options => options.UseInMemoryDatabase("ExchangeDatabase"));

            // Регистрация сервисов
            services.AddScoped<UserService>();
            services.AddScoped<CurrencyService>();
            services.AddScoped<AccountService>();
            services.AddScoped<ExchangeService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cifraex API",
                    Version = "v1",
                    Description = "API для обмена валют, управления пользователями и валютными операциями.",
                    Contact = new OpenApiContact
                    {
                        Name = "Поддержка Cifraex",
                        Email = "support@cifraex.com",
                        Url = new Uri("https://www.cifraex.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Используйте под свою ответственность",
                        Url = new Uri("https://www.cifraex.com/license")
                    }
                });
                c.OperationFilter<HideFieldsInPostRequestOperationFilter>();
            });

            services.AddAuthorization();
        }
    }
}
