using Microsoft.EntityFrameworkCore;
using Statutis.DbRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

string hostname = configuration.GetConnectionString("hostname");
string port = configuration.GetConnectionString("port");
string username = configuration.GetConnectionString("username");
string password = configuration.GetConnectionString("password");
string database = configuration.GetConnectionString("database");

builder.Services.AddDbContext<StatutisContext>(opt => opt.UseNpgsql(
    @"Host="+hostname+";Username="+username+";Password="+password+";Database="+database+""));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();