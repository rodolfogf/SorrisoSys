using Microsoft.EntityFrameworkCore;
using SorrisoSys.Data;
using SorrisoSys.Repositories.Interfaces;
using SorrisoSys.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["ConnectionStrings:SorrisoSysConnection"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();

builder.Services.AddDbContext<SorrisoSysContext>(opts =>
    opts.UseLazyLoadingProxies().UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString))
);

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