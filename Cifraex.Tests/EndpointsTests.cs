using Cifraex.Data;
using Cifraex.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cifraex.Tests;

public class EndpointTestBase
{
    protected readonly HttpClient TestClient;

    protected EndpointTestBase()
    {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<ExchangeContext>(options =>
                        options.UseInMemoryDatabase("TestDatabase"));
                });
            });

        TestClient = appFactory.CreateClient();
    }
}
public class EndpointsTests
{
    //User
    public class UserEndpointsTests : EndpointTestBase
    {
        [Fact]
        public async Task CreateUser_AddsNewUser_WhenDataIsValid()
        {
            // Arrange
            var newUser = new User { Name = "New User" };

            // Act
            var createResponse = await TestClient.PostAsJsonAsync("/api/User", newUser);

            // Assert
            createResponse.EnsureSuccessStatusCode();
            var createdUser = await createResponse.Content.ReadAsAsync<User>();
            Assert.NotNull(createdUser);
            Assert.Equal(newUser.Name, createdUser.Name);
        }

        [Fact]
        public async Task GetUserById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var expectedUser = new User { UserId = 1, Name = "Test User" };
            var createResponse = await TestClient.PostAsJsonAsync("/api/User", expectedUser);
            Assert.True(createResponse.IsSuccessStatusCode); // Проверяем, что пользователь создан успешно

            // Act
            var response = await TestClient.GetAsync($"/api/User/{expectedUser.UserId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var returnedUser = await response.Content.ReadAsAsync<User>();
            Assert.NotNull(returnedUser);
            Assert.Equal(expectedUser.UserId, returnedUser.UserId);
            Assert.Equal(expectedUser.Name, returnedUser.Name);
        }

        [Fact]
        public async Task UpdateUser_UpdatesUserData_WhenDataIsValid()
        {
            // Arrange
            var user = new User { Name = "User To Update" };
            var createUserResponse = await TestClient.PostAsJsonAsync("/api/User", user);
            var createdUser = await createUserResponse.Content.ReadAsAsync<User>();
            var updatedUserData = new { UserId = createdUser.UserId, Name = "Updated User" };

            // Act
            var updateResponse = await TestClient.PutAsJsonAsync($"/api/User/{createdUser.UserId}", updatedUserData);

            // Assert
            updateResponse.EnsureSuccessStatusCode();
            var response = await TestClient.GetAsync($"/api/User/{createdUser.UserId}");
            var updatedUser = await response.Content.ReadAsAsync<User>();
            Assert.Equal("Updated User", updatedUser.Name);
        }

        [Fact]
        public async Task DeleteUser_RemovesUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Name = "User To Delete" };
            var createUserResponse = await TestClient.PostAsJsonAsync("/api/User", user);
            var createdUser = await createUserResponse.Content.ReadAsAsync<User>();

            // Act
            var deleteResponse = await TestClient.DeleteAsync($"/api/User/{createdUser.UserId}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
            var getResponse = await TestClient.GetAsync($"/api/User/{createdUser.UserId}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }

    }

    //Currency
    public class CurrencyEndpointsTests : EndpointTestBase
    {
        [Fact]
        public async Task GetAllCurrencies_ReturnsCurrencies_WhenCurrenciesExist()
        {
            // Arrange
            var newCurrency = new Currency { CurrencyCode = "USD", Description = "Доллар США" };
            var createResponse = await TestClient.PostAsJsonAsync("/api/Currency", newCurrency);

            // Act
            var response = await TestClient.GetAsync("/api/Currency");

            // Assert
            response.EnsureSuccessStatusCode();
            var currencies = await response.Content.ReadAsAsync<List<Currency>>();
            Assert.NotNull(currencies);
            Assert.True(currencies.Any());
        }
    }

    //Exchange
    public class ExchangeEndpointsTests : EndpointTestBase
    {
        [Fact]
        public async Task ExchangeCurrency_UpdatesBalances_WhenExchangeIsValid()
        {
            // Arrange
            // Создание валют
            var currencyUSD = new Currency { CurrencyCode = "USD", Description = "Доллар США" };
            var currencyEUR = new Currency { CurrencyCode = "EUR", Description = "Евро" };
            await TestClient.PostAsJsonAsync("/api/Currency", currencyUSD);
            await TestClient.PostAsJsonAsync("/api/Currency", currencyEUR);

            // Создание пользователя
            var user = new User { Name = "Test User" };
            var createUserResponse = await TestClient.PostAsJsonAsync("/api/User", user);
            var createdUser = await createUserResponse.Content.ReadAsAsync<User>();

            // Установка начального баланса пользователя в USD
            var initialBalance = new { Currency = "USD", Amount = 100.0 };
            await TestClient.PutAsJsonAsync($"/api/User/{createdUser.UserId}/balance", initialBalance);

            // Данные для обмена валюты
            var exchangeRequest = new
            {
                UserId = createdUser.UserId, FromCurrency = "USD", ToCurrency = "EUR", Amount = 50.0, ExchangeRate = 2,
                CommissionRate = 0.05
            };

            // Act
            var exchangeResponse = await TestClient.PostAsJsonAsync($"/api/Exchange", exchangeRequest);
            var userResponse = await TestClient.GetAsync($"/api/User/{createdUser.UserId}");
            var returnedBalance = await userResponse.Content.ReadAsAsync<User>();

            // Assert
            exchangeResponse.EnsureSuccessStatusCode();
            Assert.Equal((decimal)(exchangeRequest.Amount*exchangeRequest.ExchangeRate - exchangeRequest.Amount * exchangeRequest.ExchangeRate*exchangeRequest.CommissionRate), returnedBalance.Accounts.Last().Balance);
        }
    }

}
