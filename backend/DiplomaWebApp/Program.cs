using DiplomaWebApp.Abstractions;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Repositories;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MathBattlesDbContext>(
        options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
       
    });
builder.Services.AddCors(x => x.AddPolicy("AllowOneOrigin", x =>
{
    x.AllowAnyHeader();
    x.WithOrigins("https://localhost:5173");
    x.AllowAnyMethod();
    x.AllowCredentials();
}));
builder.Services.AddAuthorization();

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IJureRepository, JureRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IRoundRepository, RoundRepository>();
builder.Services.AddScoped<BreakRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<ChangeRepository>();
builder.Services.AddScoped<RolesChangesRepository>();
builder.Services.AddScoped<MistakeRepository>();
builder.Services.AddScoped<RoundResultRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.None
}
);
app.UseCors(x => {
    x.AllowAnyHeader();
    x.WithOrigins("https://localhost:5173");
    x.AllowAnyMethod();
    x.AllowCredentials();
});
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
