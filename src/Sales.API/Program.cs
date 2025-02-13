using Sales.API.DI;
using Sales.API.Filters;
using Sales.API.Handlers;
using Sales.Application.DI;
using Sales.Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>()
                .AddProblemDetails()
                .AddInfrastructure(builder.Configuration)
                .AddApplication()
                .AddSwagger()
                .AddControllers(options => { options.Filters.Add(typeof(ValidationFilter)); })
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

app.ApplyMigrations();

if (app.Environment.IsDevelopment())
    app.UseSwagger()
       .UseSwaggerUI();

app.UseExceptionHandler()
   //.UseHttpsRedirection()
   .UseAuthorization();

app.MapControllers();

app.Run();