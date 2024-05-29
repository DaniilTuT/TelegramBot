using Application.Interfaces;
using Application.Services;
using Infrastructure.Dal.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("TelegramBotDatabase");
builder.Services.AddDbContext<TelegramBotDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<PersonServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();