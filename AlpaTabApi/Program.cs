using AlpaTabApi.Data;
using AlpaTabApi.Endpoints;
using AlpaTabApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
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

//public static readonly ILoggerFactory AlpaTabLoggerFactory
//    = LoggerFactory.Create(builder => { builder.AddConsole(); });

//var sqlConBuilder = new SqlConnectionStringBuilder();
//sqlConBuilder.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//sqlConBuilder.UserID = builder.Configuration["UserID"];
//sqlConBuilder.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<AlpaTabContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        //.UseSqlServer(builder.Configuration.GetConnectionString(sqlConBuilder.ConnectionString))
        //.UseLoggerFactory(AlpaTabLoggerFactory)
); 
builder.Services.AddScoped<IAlpaTabDataReporitory, AlpaTabDataRepository>(); 
builder.Services.AddTransient<TestDataSeeder>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    if (args.Contains("seeddata"))
        app.SeedTestData();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(AllowedOrigins);
app.UseHttpsRedirection();

app.MapGet("/", () => { return AlpaTabApi.Helpers.Constants.WELCOME_API; });
app.MapAlpaTabUsersEndpoints();
app.MapAlpaTabTransactionsEndpoints();

app.Run();

