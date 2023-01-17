using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using server;
using server.Authorization;
using server.Entities;
using server.Middleware;
using server.Models;
using server.Models.Validators;
using server.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		policy =>
		{
			policy
				.AllowAnyHeader()
				.AllowAnyOrigin()
				.AllowAnyMethod();
		});
});

// Authentication snippet

var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton<AuthenticationSettings>(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
	option.DefaultAuthenticateScheme = "Bearer";
	option.DefaultScheme = "Bearer";
	option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
	cfg.RequireHttpsMetadata = false;
	cfg.SaveToken = true;
	cfg.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = authenticationSettings.JwtIssuer,
		ValidAudience = authenticationSettings.JwtIssuer,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
	};
});


builder.Services.AddAuthorization();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ProductResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, EntryResourceOperationRequirementHandler>();

builder.Services.AddControllers()
	.AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// my
builder.Services.AddLogging();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbConnection")));
builder.Services.AddScoped<DataSeeder>();
builder.Services.AddAutoMapper(typeof(AppMappingProfile).Assembly); // Assembly.GetExecutingAssembly()
builder.Services.AddScoped<IAllergenService, AllergenService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<ISymptomService, SymptomService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IEntryService, EntryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPollinationService, PollinationService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserPasswordDto>, UpdateUserPasswordDtoValidator>();
builder.Services.AddScoped<IValidator<ForgotPasswordDto>, ForgotPasswordDtoValidator>();
builder.Services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1",
		new OpenApiInfo
		{
			Title = "Allergy Diary API",
			Version = "v1"
		});
	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
	/*c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Description = "JWT Authorization header",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement()
	{
		{
			new OpenApiSecurityScheme()
			{
				Reference = new OpenApiReference()
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				},
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});*/
});


//NLog
//builder.Logging.ClearProviders();
//builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
//builder.Host.UseNLog();

var app = builder.Build();

//DataSeeder scope
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
seeder.Seed();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    
}


app.UseMiddleware<ErrorHandlingMiddleware>();

//app.UseDeveloperExceptionPage();

app.UseAuthentication();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

/*
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});*/

app.Run();