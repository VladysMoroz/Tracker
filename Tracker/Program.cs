using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
