using Microsoft.AspNetCore.Mvc.Testing;
using AlpaTabApi.Models;
using AlpaTabApi.Helpers;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace AlpaTabApi.Tests;

public class TransactionEndpointsTests
{
    [Fact]
    [Trait("Category", Constants.TEST_PASSING)]
    public async void TestGetAllTransactions()
    {
        HttpResponseMessage response;
        HttpClient client = HttpClientHelper.CreateClient();
        response = await client.GetAsync(Constants.TRANSACTIONS_PATH);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        string transactionsString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(transactionsString);
        IEnumerable<AlpaTabTransaction> transactions = JsonConvert.DeserializeObject<IEnumerable<AlpaTabTransaction>>(transactionsString);
        Assert.Contains(transactions, _ => _.NickName == "Nicole" && _.Amount == -1);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestGetTransactionById()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.GetAsync($"{Constants.TRANSACTIONS_PATH}/1");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        var transactionString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(transactionString);
        AlpaTabTransaction actualTransaction = JsonConvert.DeserializeObject<AlpaTabTransaction>(transactionString);
        Assert.NotNull(actualTransaction);
        AlpaTabTransaction expectedTransaction = new()
        {
            NickName = "Alex",
            Amount = -4.3,
            TransactionType = "beers",
            Timestamp = DateTime.Parse("2022-10-10 02:13:53.1102898"),
        };
        AssertTransactionsEqualsIgnoreId(expectedTransaction, actualTransaction);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestCreateTransaction_WithId()
    {
        // id in payload should be discarted
        HttpClient client = HttpClientHelper.CreateClient();
        AlpaTabTransaction newTransaction = new()
        {
            Amount = 10,
            Description = "Test transaction",
            NickName = "TestUser",
            Timestamp = DateTime.Now,
            TransactionType = "TestTransaction",
            Id = 1,
        };
        var response = await client.PostAsJsonAsync($"{Constants.TRANSACTIONS_PATH}", newTransaction);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        var transactionString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(transactionString);
        AlpaTabTransaction retTransaction = JsonConvert.DeserializeObject<AlpaTabTransaction>(transactionString);
        Assert.NotNull(retTransaction);
        AssertTransactionsEqualsIgnoreId(newTransaction, retTransaction);

        response = await client.GetAsync($"{Constants.TRANSACTIONS_PATH}/{retTransaction?.Id}");
        transactionString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(transactionString);
        retTransaction = JsonConvert.DeserializeObject<AlpaTabTransaction>(transactionString);
        Assert.NotNull(retTransaction);
        AssertTransactionsEqualsIgnoreId(newTransaction, retTransaction);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestCreateDeleteTransaction()
    {
        HttpClient client = HttpClientHelper.CreateClient();

        // create transaction 
        AlpaTabTransaction newTransaction = new()
        {
            Amount = 10,
            Description = "Test transaction",
            NickName = "TestUser",
            Timestamp = DateTime.Now,
            TransactionType = "TestTransaction",
        };
        var response = await client.PostAsJsonAsync($"{Constants.TRANSACTIONS_PATH}", newTransaction);
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        var transactionString = await response.Content.ReadAsStringAsync();
        AlpaTabTransaction createdTransaction = JsonConvert.DeserializeObject<AlpaTabTransaction>(transactionString);

        // delete transaction 
        response = await client.DeleteAsync($"{Constants.TRANSACTIONS_PATH}/{createdTransaction.Id}");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        transactionString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(transactionString);
        AlpaTabTransaction deletedTransaction = JsonConvert.DeserializeObject<AlpaTabTransaction>(transactionString);
        Assert.NotNull(deletedTransaction);
        AssertTransactionsEqualsIgnoreId(deletedTransaction, createdTransaction);
        Assert.Equal(deletedTransaction.Id, createdTransaction.Id);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestModifyTransaction()
    {
        HttpClient client = HttpClientHelper.CreateClient();

        // create transaction 
        AlpaTabTransaction newTransaction = new()
        {
            Amount = 10,
            Description = "Test transaction",
            NickName = "TestUser",
            Timestamp = DateTime.Now,
            TransactionType = "TestTransaction",
        };
        var response = await client.PostAsJsonAsync($"{Constants.TRANSACTIONS_PATH}", newTransaction);
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        var transactionString = await response.Content.ReadAsStringAsync();
        AlpaTabTransaction createdTransaction = JsonConvert.DeserializeObject<AlpaTabTransaction>(transactionString);
        Assert.NotNull(createdTransaction);

        // modify transaction
        AlpaTabTransaction newTransactionModif = new()
        {
            Amount = 9090,
            Description = "Test modify transaction",
            NickName = "TestUser",
            Timestamp = DateTime.Now,
            TransactionType = "TestTransaction",
        };
        var response1 = await client.PutAsJsonAsync($"{Constants.TRANSACTIONS_PATH}/{createdTransaction.Id}", newTransactionModif);
        Assert.Equal(System.Net.HttpStatusCode.OK, response1.StatusCode);
        var transactionString1 = await response1.Content.ReadAsStringAsync();
        Assert.NotNull(transactionString1);
        AlpaTabTransaction modifiedTransaction = JsonConvert.DeserializeObject<AlpaTabTransaction>(transactionString1);
        Assert.NotNull(modifiedTransaction);
        AssertTransactionsEqualsIgnoreId(newTransactionModif, modifiedTransaction);

        // get transaction
        var response2 = await client.GetAsync($"{Constants.TRANSACTIONS_PATH}/{modifiedTransaction.Id}");
        Assert.NotNull(response2);
        Assert.Equal(System.Net.HttpStatusCode.OK, response2.StatusCode);
        var transactionString2 = await response2.Content.ReadAsStringAsync();
        Assert.NotNull(transactionString2);
        AlpaTabTransaction retTransaction = JsonConvert.DeserializeObject<AlpaTabTransaction>(transactionString2);
        Assert.NotNull(retTransaction);
        AssertTransactionsEqualsIgnoreId(newTransactionModif, retTransaction);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestGetUserTransactions()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.GetAsync($"{Constants.TRANSACTIONS_PATH}{Constants.USERS_PATH}/Alex");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        var transactionString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(transactionString);
        IEnumerable<AlpaTabTransaction> transactions = JsonConvert.DeserializeObject<IEnumerable<AlpaTabTransaction>>(transactionString);
        Assert.NotNull(transactions);
        Assert.Single(transactions);

    }


    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestGetUserTransactions_InvalidUser()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.GetAsync($"{Constants.TRANSACTIONS_PATH}/1000001");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestModifyTransaction_InvalidTransaction()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        AlpaTabTransaction newTransactionModif = new()
        {
            Amount = 9090,
            Description = "Test modify transaction",
            NickName = "TestUser",
            Timestamp = DateTime.Now,
            TransactionType = "TestTransaction",
        };
        var response = await client.PutAsJsonAsync($"{Constants.TRANSACTIONS_PATH}/1001010", newTransactionModif);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestDeleteTransaction_InvalidTransaction()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.DeleteAsync($"{Constants.TRANSACTIONS_PATH}/5000000");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestCreateTransaction_missingNickname()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        AlpaTabTransaction newTransaction = new()
        {
            Amount = 9090,
            Description = "Test modify transaction",
            Timestamp = DateTime.Now,
            TransactionType = "TestTransaction",
        };
        var response = await client.PostAsJsonAsync($"{Constants.TRANSACTIONS_PATH}", newTransaction);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestCreateTransaction_missingAmount()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        AlpaTabTransaction newTransaction = new()
        {
            NickName = "TestUser",
            Description = "Test modify transaction",
            Timestamp = DateTime.Now,
            TransactionType = "TestTransaction",
        };
        var response = await client.PostAsJsonAsync($"{Constants.TRANSACTIONS_PATH}", newTransaction);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestCreateTransaction_missingTransactionType()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        AlpaTabTransaction newTransaction = new()
        {
            NickName = "TestUser",
            Description = "Test modify transaction",
            Timestamp = DateTime.Now,
            Amount = 0,
        };
        var response = await client.PostAsJsonAsync($"{Constants.TRANSACTIONS_PATH}/4", newTransaction);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestGetUserTransactions_missingUser()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.GetAsync($"{Constants.TRANSACTIONS_PATH}{Constants.USERS_PATH}/invalid_entry");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }


    // Helpers

    internal static void AssertTransactionsEqualsIgnoreId(AlpaTabTransaction t1, AlpaTabTransaction t2)
    {
        Assert.Equal(t1.NickName, t2.NickName);
        Assert.Equal(t1.Amount.ToString("0.0"), t2.Amount.ToString("0.0"));
        Assert.Equal(t1.TransactionType, t2.TransactionType);
        Assert.Equal(0, t1.Timestamp.CompareTo(t2.Timestamp));
        Assert.Equal(t1.Description, t2.Description);
    }
}