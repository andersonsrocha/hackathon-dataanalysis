using HackathonDataAnalysis.Api.Configurations;
using HackathonDataAnalysis.Application;
using HackathonDataAnalysis.Data;
using HackathonDataAnalysis.Security;
using HackathonDataAnalysis.Api.Middlewares;
using HackathonDataAnalysis.Auth;
using HackathonDataAnalysis.NewRelicEvent;
using HackathonDataAnalysis.Plots;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

#region [Database]
builder.Services.AddMongoClient(builder.Configuration);
builder.Services.AddMongoContext();
builder.Services.AddSqlContext(builder.Configuration);
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
builder.Services.AddHostedService<ReadingQueue>();
builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
#endregion

#region [Services]
builder.Services.AddAuthService(builder.Configuration);
builder.Services.AddPlotService(builder.Configuration);
builder.Services.AddNewRelicEventPublisher(builder.Configuration);
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