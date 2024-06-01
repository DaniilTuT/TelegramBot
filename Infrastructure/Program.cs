using Application.Interfaces.Repositories;
using Application.Mapping;
using Application.Services;
using Infrastructure.Dal.EntityFramework;
using Infrastructure.Dal.Repositories;
using Infrastructure1.Jobs;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
// Добавление настроек из файла.
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<TelegramBotDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(PersonMappingProfile));
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<PersonService>();
///TODO: типизировать блок CronExpression
var testJobCronExpression = builder.Configuration["CronExpression:TestJob"];


builder.Services.AddQuartz(x =>
{
    /// найти современный вариань
    x.UseMicrosoftDependencyInjectionFactory();

    var jobKey = new JobKey("TestJob");

    x.AddJob<TestJob>(opts => opts.WithIdentity(jobKey));

    var triggerKey = new TriggerKey("TestJobTrigger");

    x.AddTrigger(opts => opts.ForJob(jobKey).WithIdentity(triggerKey)
        .WithCronSchedule(testJobCronExpression));
});
///TODO: Read about Cron, добавить cron запись в appsettings


builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobComplete = true;
});

var app = builder.Build();

// Настройка конвейера обработки HTTP-запросов.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();