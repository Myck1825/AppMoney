using ApplicationWorker;
using AppModey.Database;
using AppModey.Database.Mapper;
using AppModey.Database.Repository;
using AppModey.Database.UnitOfWork;
using AppMoney.Database;
using AppMoney.Database.Enums;
using AppMoney.Database.Mapper;
using AppMoney.Database.Options;
using AppMoney.ExceptionHandlers.Middleware;
using AppMoney.Middleware.IpAddressHandler;
using AppMoney.Model.Applications.Commands.CreateApplicationCommand;
using AppMoney.Model.Applications.Queries.GetApplicationQuery;
using AppMoney.Model.Applications.Queries.GetListByFilterApplicationQuery;
using AppMoney.Model.Decorator;
using AppMoney.Model.Mapper;
using AppMoney.Model.RabbitMQ;
using AppMoney.Model.RabbitMQ.Connection;
using AppMoney.Model.RabbitMQ.Publisher;
using AppMoney.Respose.ApiResponse;
using AppMoney.Respose.Applications;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<AlreadyExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateApplicationCommandHandler).Assembly,
                                       typeof(GetByIdApplicationQueryHandler).Assembly,
                                       typeof(GetListByFilterApplicationQueryHandler).Assembly));
builder.Services.Configure<RabbitMQOption>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddHostedService<ApplicationConsumerService>();

//var connectionString = builder.Configuration.GetValue<string>("ConnectionString");
builder.Services.Configure<DbOptions>(c=>
{
    c.ConnectionString = builder.Configuration.GetValue<string>("ConnectionString")!;
    c.DbType = builder.Configuration.GetValue<DatabaseType>("DbOptions:DbType");
});

builder.Services.AddScoped<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddScoped(typeof(IRabbitMQPublisher), typeof(RabbitMQPublisher));
builder.Services.AddSingleton<INotifyCosumer, NotifyCosumer>();

builder.Services.AddTransient<IApplicationRepository, ApplicationRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<IDbConnectionFactory, ApplicationDapperContext>();

builder.Services.Decorate<IRequestHandler<CreateApplicationCommand, ServerResponse<Guid>>, CommandHandleDecorator<CreateApplicationCommand, ServerResponse<Guid>>>();
builder.Services.Decorate<IRequestHandler<GetByIdApplicationQuery, ServerResponse<ApplicationResponse>>, CommandHandleDecorator<GetByIdApplicationQuery, ServerResponse<ApplicationResponse>>>();
builder.Services.Decorate<IRequestHandler<GetListByFilterApplicationQuery, ServerResponse<IReadOnlyList<ApplicationResponse>>>, CommandHandleDecorator<GetListByFilterApplicationQuery, ServerResponse<IReadOnlyList<ApplicationResponse>>>>();

builder.Services.AddAutoMapper(typeof(RequestToEntityMapper).Assembly,
    typeof(EntityToResponseMapper).Assembly);
builder.Services.AddSingleton(new MapperConfiguration(conf =>
{
    conf.AddProfile(new RequestToEntityMapper());
    conf.AddProfile(new EntityToResponseMapper());
    conf.AddProfile(new DbResultToServerResponseMapper());
}));

var app = builder.Build();
app.UseExceptionHandler();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseMiddleware<IpAddressHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
