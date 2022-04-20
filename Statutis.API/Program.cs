using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Add Authentification
var symKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("JWT:secret"));
builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = false,
		IssuerSigningKey = new SymmetricSecurityKey(symKey),
		ValidateIssuerSigningKey = false,
		ValidateAudience = false
	};
});

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo(){Title = "Statutis API", Version = "v1"});
	options.AddSecurityDefinition("Bearer Authentication", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization using Bearer token",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		BearerFormat = "JWT"
	});
});

builder.Services.AddDbRepositories();
builder.Services.AddBusiness();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder =>
	{
		builder
			.WithOrigins(configuration.GetSection("Application").GetSection("origin").Get<string[]>())
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

var app = builder.Build();

app.UseCors();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();