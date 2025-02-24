using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tracker.Controllers.Validation;
using Tracker.DatabaseCatalog.Repositories;
using Tracker.Interfaces;
using Tracker.Repositories;
using Tracker.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ValidationPatterns>(builder.Configuration.GetSection("ValidationPatterns"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// Services
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddSingleton<ValidationService>();

builder.Services.AddDbContext<TrackerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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
