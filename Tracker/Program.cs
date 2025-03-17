using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tracker.Controllers.AutoMappers;
using Tracker.Controllers.Validation;
using Tracker.DatabaseCatalog.Repositories;
using Tracker.Entitites.Filters;
using Tracker.Interfaces.RepositoryInterfaces;
using Tracker.Interfaces.ServiceInterfaces;
using Tracker.Repositories;
using Tracker.Services;

var builder = WebApplication.CreateBuilder(args);


// FILTERS
builder.Services.Configure<ValidationPatterns>(builder.Configuration.GetSection("ValidationPatterns"));
builder.Services.AddScoped<ValidateCategoryNameFilter>();
builder.Services.AddScoped<ValidateFilterAttribute>();
builder.Services.AddScoped<ValidateNewNameAndIdFilter>();
builder.Services.AddSingleton<ValidationService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//AUTOMAPPERS
builder.Services.AddAutoMapper(
     typeof(FromCreateGoalViewModelToGoalMapper),
     typeof(FromEditGoalViewModelToGoalMapper));

//REPOSITORIES
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();

// SERVICES
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IGoalService, GoalService>();

builder.Services.AddDbContext<TrackerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:5085";
        options.Audience = "timetracker_api";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "http://localhost:5085",
            ValidAudience = "timetracker_api",

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),

            //RoleClaimType = "role", // Скажи де шукати роль
            //NameClaimType = "nameid" // За потреби для NameIdentifier
        };
    });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
