using Microsoft.EntityFrameworkCore;
using Statutis.API.Utils.DependencyInjection;
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
	@"Host=" + hostname + ";Username=" + username + ";Password=" + password + ";Database=" + database + ""));

builder.Services.AddDbRepositories();
builder.Services.AddBusiness();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//run migrations
using (var scope = app.Services.CreateScope())
{
	var dataContext = scope.ServiceProvider.GetRequiredService<StatutisContext>();
	dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    
}*/

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();