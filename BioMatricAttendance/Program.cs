using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Repositories;
using BioMatricAttendance.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
builder.Services.AddScoped<IAttendanceProcessingService, AttendanceProcessingService>();
builder.Services.AddScoped<IAttendanceProcessingRepository, AttendanceProcessingRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBioMatricDeviceRepository, BioMatricDeviceRepository>();
builder.Services.AddScoped<IBioMatricDeviceService, BioMatricDeviceService>();
builder.Services.AddScoped<IInstituteRepository, InstituteRepository>();
builder.Services.AddScoped<IInstituteService, InstituteService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder
        .WithOrigins(new string[]
        {
            "http://localhost:5173",
            "https://wonderful-florentine-84e2e4.netlify.app/",




        })
        .WithMethods("POST", "PUT", "DELETE", "GET")
        .AllowAnyHeader()
        .AllowCredentials();
    });
});


var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()||app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
