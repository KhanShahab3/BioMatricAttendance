using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Repositories;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IInstituteAttendanceRepository,InstituteAttendanceRepository>();
builder.Services.AddScoped<IInstituteDashboardService,InstituteDashboardService>();
builder.Services.AddScoped<IRegionDashboardRepository,RegionDashboardRepository>();
builder.Services.AddScoped<ICourseCandidatesRepository, CourseCandidateRepository>();
builder.Services.AddScoped<ICourseCandidateService,CouserCandidateService>();
builder.Services.AddScoped<IRegionDashboardService,RegionDashboardService>();
builder.Services.AddScoped<IDistrictRepository,DistrictRepository>();
builder.Services.AddScoped<IDistrictService,DistrictService>();
builder.Services.AddScoped<JwtService>();


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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();


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


