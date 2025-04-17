using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.Data.Extensions;
using Server.Data.Repositories;
using Server.Middleware;
using Server.Models.Interfaces;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos;
using Server.Services.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddData(builder.Configuration);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UsersService>();

builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<FacultiesService>();

builder.Services.AddScoped<IHoldingRepository, HoldingRepository>();
builder.Services.AddScoped<HoldingsService>();

builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
builder.Services.AddScoped<IWorkerDtoRepository, WorkerDtoRepository>();
builder.Services.AddScoped<WorkersService>();

builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<SpecialtiesService>();

builder.Services.AddScoped<IAcademicianRepository, AcademicianRepository>();
builder.Services.AddScoped<AcademiciansService>();

builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<GroupsService>();

builder.Services.AddScoped<IDisciplineRepository, DisciplineRepository>();
builder.Services.AddScoped<IDisciplineDtoRepository, DisciplineDtoRepository>();
builder.Services.AddScoped<DisciplinesService>();

builder.Services.AddScoped<IRecordRepository, RecordRepository>();
builder.Services.AddScoped<IRecordDtoRepository, RecordDtoRepository>();
builder.Services.AddScoped<RecordsService>();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<StudentsService>();

builder.Services.Configure<MailOptions>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<DisciplineStatusThresholds>(builder.Configuration.GetSection("DisciplineStatusThresholds"));
builder.Services.Configure<DisciplineStatusColors>(builder.Configuration.GetSection("DisciplineStatusColors"));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtOptions>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            ClockSkew = TimeSpan.FromMinutes(2),
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<LoggingMiddleware>();

app.MapControllers();

app.Run();
