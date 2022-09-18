using AlpaTabApi.Data;
using AlpaTabApi.Models;
using Microsoft.EntityFrameworkCore;

var AllowedOrigins = "_allowedOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedOrigins,
                      policy =>
                      {
                          //policy.WithOrigins("http://192.168.1.174:3000").AllowAnyMethod().AllowAnyHeader();
                          policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                      });
});

builder.Services.AddDbContext<AlpaTabContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

app.UseCors(AllowedOrigins);
app.UseHttpsRedirection();

app.MapGet("/", () => { return "Welcome to the AlpaTab api!"; });

app.MapGet("/users", async (AlpaTabContext context) => await context.AlpaTabUsers.Select(user => new AlpaTabUserDTO(user)).ToListAsync());

app.MapGet("/users/{id}", async (AlpaTabContext context, int id) =>
    await context.AlpaTabUsers.FindAsync(id) is AlpaTabUser user ? Results.Ok(new AlpaTabUserDTO(user)) : Results.NotFound($"User not found: {id}"));

app.MapPost("/users", async (AlpaTabContext context, AlpaTabUser user) =>
{
    context.AlpaTabUsers.Add(user);
    await context.SaveChangesAsync();
    return Results.Ok(new AlpaTabUserDTO(user));
});

app.MapPut("/users/{id}", async (AlpaTabContext context, AlpaTabUser user, int id) =>
{
    AlpaTabUser existingUsr = await context.AlpaTabUsers.FindAsync(id);
    if (existingUsr == null) return Results.NotFound($"User not found: {id}");

    existingUsr.FirstName = user.FirstName;
    existingUsr.LastName = user.LastName;
    existingUsr.UserType = user.UserType;
    existingUsr.Password = user.Password; // TODO: add some kind of constraint on that
    existingUsr.NickName = user.NickName;
    existingUsr.Email = user.Email;
    await context.SaveChangesAsync();

    return Results.Ok(new AlpaTabUserDTO(user));
});

app.MapDelete("/users/{id}", async (AlpaTabContext context, int id) =>
{
    AlpaTabUser userToRemove = await context.AlpaTabUsers.FindAsync(id);
    if (userToRemove == null) return Results.NotFound($"User not found: {id}");

    context.AlpaTabUsers.Remove(userToRemove);
    await context.SaveChangesAsync();
    return Results.Ok(new AlpaTabUserDTO(userToRemove));
});


app.MapGet("/transactions", async (AlpaTabContext context) => await context.TransactionsList.ToListAsync());

app.MapGet("/transactions/{id}", async (AlpaTabContext context, int id) =>
    await context.TransactionsList.FindAsync(id) is AlpaTabTransaction t ? Results.Ok(t) : Results.NotFound($"Transaction not found: {id}"));

app.MapGet("/transactions/{nickname}", async (AlpaTabContext context, string nickName) =>
{
    IList<AlpaTabTransaction> transactions = await context.TransactionsList.Where(_ => _.NickName == nickName).ToListAsync();
    return Results.Ok(transactions);
});

app.MapPost("/transactions", async (AlpaTabContext context, AlpaTabTransaction t) =>
{
    context.TransactionsList.Add(t);
    await context.SaveChangesAsync();
    return Results.Ok(t);
});

app.MapPut("/transactions/{id}", async (AlpaTabContext context, AlpaTabTransaction t, int id) =>
{
    AlpaTabTransaction existingT = await context.TransactionsList.FindAsync(id);
    if (existingT == null) return Results.NotFound($"Transaction not found: {id}");

    existingT.NickName = t.NickName;
    existingT.Amount = t.Amount;
    existingT.TransactionType = t.TransactionType;
    existingT.Timestamp = t.Timestamp;
    existingT.Description = t.Description;
    await context.SaveChangesAsync();

    return Results.Ok(existingT);
});

app.MapDelete("/transactions/{id}", async (AlpaTabContext context, int id) =>
{
    AlpaTabTransaction tToRemove = await context.TransactionsList.FindAsync(id);
    if (tToRemove == null) return Results.NotFound($"Transaction not found: {id}");

    context.TransactionsList.Remove(tToRemove);
    await context.SaveChangesAsync();
    return Results.Ok(tToRemove);
});

app.Run();
