using Microsoft.AspNetCore.Mvc.Testing;
using AlpaTabApi.Models;
using System.Net.Http.Json;

namespace AlpaTabApi.Tests;

public class TransactionEndpointsTests
{
    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestGetAllTransactions()
    {
        HttpResponseMessage response;
        HttpClient client = HttpClientHelper.CreateClient();
        response = await client.GetAsync("/transactions");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestGetTransactionById()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.GetAsync("/transaction/1");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestCreateTransaction()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.PostAsJsonAsync("/transactions", new AlpaTabTransaction
        {
            Amount = 10,
            Description = "Test transaction",
            NickName = "TestUser",
            Timestamp = DateTime.Now,
            TransactionType = "TestTransaction",
        });
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestCreateTransaction_WithId()
    {
        // id in payload should be discarted
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.PostAsJsonAsync("/transactions", new AlpaTabTransaction
        {
            Amount = 10,
            Description = "Test transaction",
            NickName = "TestUser",
            Timestamp = DateTime.Now,
            TransactionType = "TestTransaction",
            Id = 1, 
        });
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        var id = response.Content;
        var transaction = await client.GetAsync("/transactions/24");
        var test = transaction.Content;
        Assert.Equal(System.Net.HttpStatusCode.OK, transaction.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public void TestCreateTransaction_WithInitialBalance()
    {
        // should discart initial balance and set balance to 0
        throw new NotImplementedException();
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public void TestDeleteCreateTransaction()
    {

        throw new NotImplementedException();
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public void TestModifyTransaction()
    {
        throw new NotImplementedException();
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public void TestGetUserTransactions()
    {
        throw new NotImplementedException();
    }


    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public void TestGetUserTransactions_InvalidUser()
    {
        throw new NotImplementedException();
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public void TestModifyTransaction_InvalidTransaction()
    {
        throw new NotImplementedException();
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public void TestDeleteTransaction_InvalidTransaction()
    {
        throw new NotImplementedException();
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public void TestCreateTransaction_missingNickname()
    {
        throw new NotImplementedException();
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public void TestCreateTransaction_missingAmount()
    {
        throw new NotImplementedException();
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public void TestCreateTransaction_missingTransactionType()
    {
        throw new NotImplementedException();
    }
}