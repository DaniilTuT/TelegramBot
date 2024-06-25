using System.Runtime.Intrinsics.X86;
using Application.Interfaces.Repositories;
using Application.Mapping;
using Application.Services;
using Infrastructure.Dal.EntityFramework;
using Infrastructure.Dal.Repositories;
using Infrastructure;
using Infrastructure.Jobs;
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
var cronExpressionSettings = builder.Configuration.GetSection("CronExpressions").Get<CronExpressionsSettings>();
builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection("TelegramSettings"));
builder.Services.AddQuartz(x =>
{
    var jobKey = new JobKey("PersonFindBirthdaysJob");
    var telegramCreateJobKey = new JobKey("TelegramCreatePersonJob");
       
    x.AddJob<PersonFindBirthdaysJob>(opts => opts.WithIdentity(jobKey));
    x.AddJob<PersonTelegramHandlerJob>(opts => opts.WithIdentity(telegramCreateJobKey));

    
    var triggerKey = new TriggerKey("PersonFindBirthdaysJobTrigger");
    var telegramCreateTriggerKey = new TriggerKey("TelegramCreatePersonJob");
    
    x.AddTrigger(opts => opts.ForJob(telegramCreateJobKey).WithIdentity(telegramCreateTriggerKey));
    
    x.AddTrigger(opts => opts.ForJob(jobKey).WithIdentity(triggerKey)
        .WithCronSchedule(cronExpressionSettings.PersonFindBirthdaysJob));
});

builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
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