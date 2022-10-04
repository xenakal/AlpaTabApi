using AlpaTabApi.Data;
using AlpaTabApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var AllowedOrigins = "_allowedOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
     {
         c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
         c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
         {
             ValidAudience = builder.Configuration["Auth0:Audience"],
             ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
         };
     });
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("CanViewAllUsers", p => p.
        RequireAuthenticatedUser().
        RequireClaim("scope", "read:any_user"));
});


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedOrigins,
                      policy =>
                      {
                          //policy.WithOrigins("http://192.168.1.174:3000").AllowAnyMethod().AllowAnyHeader();
                          policy.WithOrigins(builder.Configuration["AllowedHosts"]).AllowAnyMethod().AllowAnyHeader();
                      });
});

builder.Services.AddDbContext<AlpaTabContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IAlpaTabDataReporitory, AlpaTabDataRepository>();
builder.Services.AddTransient<TestDataSeeder>();


var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

static void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<TestDataSeeder>();
        service.SeedData();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(AllowedOrigins);
app.UseHttpsRedirection();

app.MapGet("/", () => { return "Welcome to the AlpaTab api!!"; });

app.MapGet("/users", async (IAlpaTabDataReporitory reporitory) => await reporitory.GetAllUsers());
    //.RequireAuthorization("CanViewAllUsers");

app.MapGet("/users/{id}", async (IAlpaTabDataReporitory reporitory, int id) => await reporitory.GetUserById(id));

app.MapPost("/users", async (IAlpaTabDataReporitory repository, AlpaTabUser user) => await repository.CreateUser(user));

app.MapPut("/users/{id}", async (IAlpaTabDataReporitory repository, AlpaTabUser user, int id) => await repository.ModifyUser(id, user));

app.MapDelete("/users/{id}", async (IAlpaTabDataReporitory repository, int id) => await repository.DeleteUser(id));

app.MapGet("/transactions", async (IAlpaTabDataReporitory reporitory) => await reporitory.GetAllTransactions());
    //.RequireAuthorization("CanViewAllUsers");

app.MapGet("/transactions/{id}", async (IAlpaTabDataReporitory reporitory, int id) => await reporitory.GetUserById(id));

app.MapPost("/transactions", async (IAlpaTabDataReporitory repository, AlpaTabTransaction transaction) => await repository.CreateTransaction(transaction));

app.MapPut("/transactions/{id}", async (IAlpaTabDataReporitory repository, AlpaTabTransaction transaction, int id) => await repository.ModifyTransaction(id, transaction));

app.MapDelete("/transactions/{id}", async (IAlpaTabDataReporitory repository, int id) => await repository.DeleteTransaction(id));

app.MapGet("/transactions/user/{nickname}", async (IAlpaTabDataReporitory repository, string nickName) => await repository.GetUserTransactions(nickName));


app.Run();
