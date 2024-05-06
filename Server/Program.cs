// - koppla in en databas
// - göra post
// - kunna få in data via vår path
// - bryta ut funktioner och klasser till andra filer
// - error handling & status codes

// -- session hantering / auths --
// - login 
// - skydda routes
// - veta vem som gör en request

using Npgsql;
using Server;
// creates a connection to postgres database
string connection = "Host=localhost;Username=postgres;Password=postgres;Database=aspnet_demo;Port=5455";
await using var db = NpgsqlDataSource.Create(connection);
// creates an instance of the State record using the db connection
State state = new(db);
// setting up a workspace to create a website 
var builder = WebApplication.CreateBuilder(args);
//sets up a system where people need a special cookie to access certain parts of the website
builder.Services.AddAuthentication().AddCookie("sys23m.teachers.aspnetdemo");
builder.Services.AddAuthorizationBuilder().AddPolicy("admin", policy => policy.RequireRole("admin"));
builder.Services.AddSingleton(state);
var app = builder.Build();

app.MapGet("/users", Users.All);
app.MapGet("/users{id}", Users.Single);
app.MapPost("/users", Users.Post).RequireAuthorization("admin");
app.MapPost("/login", Auth.Login);

app.Run("http://localhost:3000");
public record State(NpgsqlDataSource DB);