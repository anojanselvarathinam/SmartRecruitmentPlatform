using Microsoft.EntityFrameworkCore;
using SmartRecruitmentPlatform.Backend.Data;
using SmartRecruitmentPlatform.Backend.Services.Interfaces;
using SmartRecruitmentPlatform.Backend.Services.Implementations;
using SmartRecruitmentPlatform.Backend.Repositories.Interfaces;
using SmartRecruitmentPlatform.Backend.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Backend/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("Backend/appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI
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

