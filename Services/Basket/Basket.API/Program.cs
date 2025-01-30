using Asp.Versioning;
using Basket.Core.Interfaces;
using Basket.Infrastructure.GrpcService;
using Basket.Infrastructure.Mappers;
using Basket.Infrastructure.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddControllers();

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
	options.ReportApiVersions = true;
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new ApiVersion(1, 0);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Basket.API", Version = "v1" }); });

//Register AutoMapper
builder.Services.AddAutoMapper(typeof(BasketMappingProfile).Assembly);

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

//Register Application Services
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(cfg => cfg.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));

builder.Services.AddMassTransit(config =>
{
	config.UsingRabbitMq((ct, cfg) =>
	{
		cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
	});
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
