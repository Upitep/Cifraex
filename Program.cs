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
            // ���������� ��������� ���� ������
            services.AddDbContext<ExchangeContext>(options => options.UseInMemoryDatabase("ExchangeDatabase"));

            // ����������� ��������
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
                    Description = "API ��� ������ �����, ���������� �������������� � ��������� ����������.",
                    Contact = new OpenApiContact
                    {
                        Name = "��������� Cifraex",
                        Email = "support@cifraex.com",
                        Url = new Uri("https://www.cifraex.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "����������� ��� ���� ���������������",
                        Url = new Uri("https://www.cifraex.com/license")
                    }
                });
                c.OperationFilter<HideFieldsInPostRequestOperationFilter>();
            });

            services.AddAuthorization();
        }
    }
}
