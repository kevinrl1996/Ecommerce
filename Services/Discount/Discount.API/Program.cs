using Common.Logging;
using Discount.API.Services;
using Discount.Core.Interfaces;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Mappers;
using Discount.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);

//Register AutoMapper
builder.Services.AddAutoMapper(typeof(DiscountMappingProfile).Assembly);

//Register Application Services
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

builder.Services.AddGrpc();

var app = builder.Build();

//Migrate Database
app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
	endpoints.MapGrpcService<DiscountService>();

	endpoints.MapGet("/", async context =>
	{
		await context.Response.WriteAsync("La comunicación con los endpoints de grpc debe realizarse a través de un cliente de grpc");
	});
});

app.Run();
