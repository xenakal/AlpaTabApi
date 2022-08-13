using AlpaTabApi.Data;
using AlpaTabApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AlpaTabContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => { return "Welcome to the api!"; });

app.MapGet("/users", async (AlpaTabContext context) => await context.AlpaTabUsers.ToListAsync());

app.MapGet("/users/{id}", async (AlpaTabContext context, int id) =>
    await context.AlpaTabUsers.FindAsync(id) is AlpaTabUser user ? Results.Ok(user) : Results.NotFound($"User not found: {id}"));

app.MapPost("/users", async (AlpaTabContext context, AlpaTabUser user) =>
{
    context.AlpaTabUsers.Add(user);
    await context.SaveChangesAsync();
    return Results.Ok(user);
});

app.MapPut("/users/{id}", async (AlpaTabContext context, AlpaTabUser user, int id) =>
{
    AlpaTabUser existingUsr = await context.AlpaTabUsers.FindAsync(id);
    if (existingUsr == null) return Results.NotFound($"User not found: {id}");

    existingUsr.FirstName = user.FirstName;
    existingUsr.LastName = user.LastName;
    existingUsr.UserType = user.UserType;
    existingUsr.Password = user.Password;
    existingUsr.NickName = user.NickName;
    existingUsr.Email = user.Email;
    await context.SaveChangesAsync();

    return Results.Ok(user);
});

app.MapDelete("/users/{id}", async (AlpaTabContext context, int id) =>
{
    AlpaTabUser userToRemove = await context.AlpaTabUsers.FindAsync(id);
    if (userToRemove == null) return Results.NotFound($"User not found: {id}");

    context.AlpaTabUsers.Remove(userToRemove);
    await context.SaveChangesAsync();
    return Results.Ok(userToRemove);
});


app.MapGet("/transactions", async (AlpaTabContext context) => await context.TransactionsList.ToListAsync());

app.MapGet("/transactions/{id}", async (AlpaTabContext context, int id) =>
    await context.TransactionsList.FindAsync(id) is AlpaTabTransaction t ? Results.Ok(t) : Results.NotFound($"Transaction not found: {id}"));

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
