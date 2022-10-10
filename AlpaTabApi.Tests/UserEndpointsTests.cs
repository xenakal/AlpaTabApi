using Microsoft.AspNetCore.Mvc.Testing;
using AlpaTabApi.Models;
using AlpaTabApi.Helpers;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace AlpaTabApi.Tests;

public class UserEndpointsTests
{
    [Fact]
    [Trait("Category", Constants.TEST_PASSING)]
    public async void TestGetAllUsers()
    {
        HttpResponseMessage response;
        HttpClient client = HttpClientHelper.CreateClient();
        response = await client.GetAsync(Constants.USERS_PATH);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        string usersString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(usersString);
        IEnumerable<AlpaTabUser> users = JsonConvert.DeserializeObject<IEnumerable<AlpaTabUser>>(usersString);
        Assert.Contains(users, _ => _.NickName == "Nicole");
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestGetUserById()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.GetAsync($"{Constants.USERS_PATH}/1");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        var userString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(userString);
        AlpaTabUser actualUser = JsonConvert.DeserializeObject<AlpaTabUser>(userString);
        Assert.NotNull(actualUser);
        AlpaTabUser expectedUser = new()
        {
            NickName = "Alex",
            FirstName = "Alex", 
            LastName = "Xela", 
            Email = "Xela@mail.com", 
            UserType = 0, 
            Balance = 0,
        };
        AssertUsersEqualsIgnoreId(expectedUser, actualUser);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestCreateUser_WithId()
    {
        // id in payload should be discarted
        HttpClient client = HttpClientHelper.CreateClient();
        AlpaTabUser newUser = new()
        {
            NickName = "Alex",
            FirstName = "Alex", 
            LastName = "Xela", 
            Email = "Xela@gmail.com", 
            UserType = 0, 
            Balance = 0,
        };
        var response = await client.PostAsJsonAsync($"{Constants.USERS_PATH}", newUser);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        var userString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(userString);
        AlpaTabUser retUser = JsonConvert.DeserializeObject<AlpaTabUser>(userString);
        Assert.NotNull(retUser);
        AssertUsersEqualsIgnoreId(newUser, retUser);

        response = await client.GetAsync($"{Constants.USERS_PATH}/{retUser?.Id}");
        userString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(userString);
        retUser = JsonConvert.DeserializeObject<AlpaTabUser>(userString);
        Assert.NotNull(retUser);
        AssertUsersEqualsIgnoreId(newUser, retUser);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestCreateDeleteUser()
    {
        HttpClient client = HttpClientHelper.CreateClient();

        // create user 
        AlpaTabUser newUser = new()
        {
            NickName = "Alex",
            FirstName = "Alex", 
            LastName = "Xela", 
            Email = "Xela@gmail.com", 
            UserType = 0, 
            Balance = 0,
        };
        var response = await client.PostAsJsonAsync($"{Constants.USERS_PATH}", newUser);
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        var userString = await response.Content.ReadAsStringAsync();
        AlpaTabUser createdUser = JsonConvert.DeserializeObject<AlpaTabUser>(userString);

        // delete user 
        response = await client.DeleteAsync($"{Constants.USERS_PATH}/{createdUser.Id}");
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        userString = await response.Content.ReadAsStringAsync();
        Assert.NotNull(userString);
        AlpaTabUser deletedUser = JsonConvert.DeserializeObject<AlpaTabUser>(userString);
        Assert.NotNull(deletedUser);
        AssertUsersEqualsIgnoreId(deletedUser, createdUser);
        Assert.Equal(deletedUser.Id, createdUser.Id);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_PASSING)]
    public async void TestModifyUser()
    {
        HttpClient client = HttpClientHelper.CreateClient();

        // create user 
        AlpaTabUser newUser = new()
        {
            NickName = "Alex",
            FirstName = "Alex", 
            LastName = "Xela", 
            Email = "Xela@gmail.com", 
            UserType = 0, 
            Balance = 0,
        };
        var response = await client.PostAsJsonAsync($"{Constants.USERS_PATH}", newUser);
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        var userString = await response.Content.ReadAsStringAsync();
        AlpaTabUser createdUser = JsonConvert.DeserializeObject<AlpaTabUser>(userString);
        Assert.NotNull(createdUser);

        // modify user
        AlpaTabUser newUserModif = new()
        {
            NickName = "Alex",
            FirstName = "Alex", 
            LastName = "Xela", 
            Email = "Xela@gmail.com", 
            UserType = 0, 
            Balance = 0,
        };
        var response1 = await client.PutAsJsonAsync($"{Constants.USERS_PATH}/{createdUser.Id}", newUserModif);
        Assert.Equal(System.Net.HttpStatusCode.OK, response1.StatusCode);
        var userString1 = await response1.Content.ReadAsStringAsync();
        Assert.NotNull(userString1);
        AlpaTabUser modifiedUser = JsonConvert.DeserializeObject<AlpaTabUser>(userString1);
        Assert.NotNull(modifiedUser);
        AssertUsersEqualsIgnoreId(newUserModif, modifiedUser);

        // get user
        var response2 = await client.GetAsync($"{Constants.USERS_PATH}/{modifiedUser.Id}");
        Assert.NotNull(response2);
        Assert.Equal(System.Net.HttpStatusCode.OK, response2.StatusCode);
        var userString2 = await response2.Content.ReadAsStringAsync();
        Assert.NotNull(userString2);
        AlpaTabUser retUser = JsonConvert.DeserializeObject<AlpaTabUser>(userString2);
        Assert.NotNull(retUser);
        AssertUsersEqualsIgnoreId(newUserModif, retUser);
    }


    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestGetUserUsers_InvalidUser()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.GetAsync($"{Constants.USERS_PATH}/1000001");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestModifyUser_InvalidUser()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        AlpaTabUser newUserModif = new()
        {
            NickName = "Alex",
            FirstName = "Alex", 
            LastName = "Xela", 
            Email = "Xela@gmail.com", 
            UserType = 0, 
            Balance = 0,
        };
        var response = await client.PutAsJsonAsync($"{Constants.USERS_PATH}/1001010", newUserModif);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestDeleteUser_InvalidUser()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        var response = await client.DeleteAsync($"{Constants.USERS_PATH}/5000000");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestCreateUser_missingNickname()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        AlpaTabUser newUser = new()
        {
            NickName = "Alex",
            FirstName = "Alex", 
            LastName = "Xela", 
            Email = "Xela@gmail.com", 
            UserType = 0, 
            Balance = 0,
        };
        var response = await client.PostAsJsonAsync($"{Constants.USERS_PATH}", newUser);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestCreateUser_missingAmount()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        AlpaTabUser newUser = new()
        {
            NickName = "Alex",
            FirstName = "Alex", 
            LastName = "Xela", 
            Email = "Xela@gmail.com", 
            UserType = 0, 
            Balance = 0,
        };
        var response = await client.PostAsJsonAsync($"{Constants.USERS_PATH}", newUser);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [Trait("Category", Helpers.Constants.TEST_FAILURE)]
    public async void TestCreateUser_missingUserType()
    {
        HttpClient client = HttpClientHelper.CreateClient();
        AlpaTabUser newUser = new()
        {
            NickName = "Alex",
            FirstName = "Alex", 
            LastName = "Xela", 
            Email = "Xela@gmail.com", 
            UserType = 0, 
            Balance = 0,
        };
        var response = await client.PostAsJsonAsync($"{Constants.USERS_PATH}/4", newUser);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }


    // Helpers

    internal static void AssertUsersEqualsIgnoreId(AlpaTabUser t1, AlpaTabUser t2)
    {
        Assert.Equal(t1.NickName, t2.NickName);
        Assert.Equal(t1.Balance.ToString("0.0"), t2.Balance.ToString("0.0"));
        Assert.Equal(t1.UserType, t2.UserType);
        Assert.Equal(t1.Email, t2.Email);
        Assert.Equal(t1.FirstName, t2.FirstName);
        Assert.Equal(t1.LastName, t2.LastName);
    }
}