using Cifraex.Models;

namespace Cifraex.Tests;

public class ModelTests
{

    //Currency
    [Fact]
    public void CreateCurrency_ShouldCreateSuccessfully()
    {
        // Arrange
        var currency = new Currency
        {
            CurrencyCode = "EUR",
            Description = "Euro"
        };

        // Assert
        Assert.Equal("EUR", currency.CurrencyCode);
        Assert.Equal("Euro", currency.Description);
    }

    //User

    [Fact]
    public void CreateUser_ShouldCreateSuccessfully()
    {
        // Arrange
        var user = new User
        {
            UserId = 1,
            Name = "Test User",
            Accounts = new List<Account>()
        };

        // Assert
        Assert.Equal(1, user.UserId);
        Assert.Equal("Test User", user.Name);
        Assert.Empty(user.Accounts);
    }

    [Fact]
    public void AddAccountToUser_ShouldAddSuccessfully()
    {
        // Arrange
        var user = new User { UserId = 1, Accounts = new List<Account>() };
        var currency = new Currency
        {
            CurrencyCode = "EUR",
            Description = "Euro"
        };
        var account = new Account { AccountId = 1, Currency = currency };

        // Act
        user.Accounts.Add(account);

        // Assert
        Assert.Contains(account, user.Accounts);
    }

    //Account
    [Fact]
    public void CreateAccount_ShouldCreateSuccessfully()
    {
        // Arrange
        var currency = new Currency
        {
            CurrencyCode = "EUR",
            Description = "Euro"
        };

        var account = new Account
        {
            AccountId = 1,
            Currency = currency,
            Balance = 100
        };

        // Assert
        Assert.Equal(1, account.AccountId);
        Assert.Equal("EUR", account.Currency.CurrencyCode);
        Assert.Equal(100, account.Balance);
    }

}