using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Server.Data.Extensions;
using Server.Data.Repositories;
using Server.Data.Repositories.DisciplineRepositories;
using Server.Data.Repositories.RecordRepositories;
using Server.Data.Repositories.StudentRepositories;
using Server.Data.Repositories.WorkerRepositroies;
using Server.Handlers;
using Server.Middleware;
using Server.Models.Interfaces;
using Server.Services.DtoInterfaces;
using Server.Services.Dtos.DisciplineDtos;
using Server.Services.Options.ContextOptions.RequestContext;
using Server.Services.Options.SettingsOptions;
using Server.Services.Services;
using Server.Services.Services.AuthorizationServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(o => o.CustomizeProblemDetails = context =>
{
    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddData(builder.Configuration);

builder.Services.AddScoped<IRequestContext, RequestContext>();

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
builder.Services.AddScoped<IStudentDtoRepository, StudentDtoRepository>();
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<RequestContextMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
