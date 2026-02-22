using HackathonDataAnalysis.Api.Configurations;
using HackathonDataAnalysis.Application;
using HackathonDataAnalysis.Data;
using HackathonDataAnalysis.Security;
using HackathonDataAnalysis.Api.Middlewares;
using HackathonDataAnalysis.Application.Readings.Handlers;
using HackathonDataAnalysis.Auth;
using HackathonDataAnalysis.Plots;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

#region [Database]
builder.Services.AddMongoContext(builder.Configuration);
builder.Services.AddRepositories();
#endregion

#region [JWT]
builder.Services.AddSecurity(builder.Configuration);
#endregion

#region [AutoMapper]
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
#endregion

#region [Mediator]
builder.Services.AddMediator();
#endregion

#region [Serilog]
builder.AddSerilog();
#endregion

#region [RabbitMQ]
builder.Services.AddHostedService<CreateReadingQueue>();
#endregion

#region [Services]
builder.Services.AddAuthService(builder.Configuration);
builder.Services.AddPlotService(builder.Configuration);
#endregion

var app = builder.Build();

app.MapSwagger("/openapi/{documentName}.json");
app.MapScalarApiReference();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();