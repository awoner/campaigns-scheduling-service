using CampaignsSchedulingService.Api.BackgroundJobs;
using CampaignsSchedulingService.Application.Services;
using CampaignsSchedulingService.Domain.Entities;
using CampaignsSchedulingService.Infrastructure.Dals;
using CampaignsSchedulingService.Infrastructure.Persistence.Convertors;
using CampaignsSchedulingService.Infrastructure.Persistence.Repositories;
using CampaignsSchedulingService.Infrastructure.Providers;
using CampaignsSchedulingService.Infrastructure.Scheduling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHostedService<CampaignsSendingJob>();
builder.Services.AddHostedService<DailyCustomerClearingJob>();

builder.Services.AddSingleton<ICsvFileProvider<CustomerDal>, CustomerCsvProvider>();
builder.Services.AddScoped<IRepository<Customer>, CustomerCsvRepository>();
builder.Services.AddScoped<IConvertor<Customer, CustomerDal>, CustomerConvertor>();

builder.Services.AddSingleton<ICsvFileProvider<CampaignDal>, CampaignCsvProvider>();
builder.Services.AddSingleton<IRepository<Campaign>, CampaignInMemoryRepository>();
builder.Services.AddScoped<ISender<Campaign>, CampaignSender>();
builder.Services.AddScoped<IConvertor<Campaign, CampaignDal>, CampaignConvertor>();
builder.Services.AddScoped<ISchedulingProcessor<Campaign>, CampaignSchedulingProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();