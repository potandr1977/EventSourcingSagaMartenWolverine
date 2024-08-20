using Application.SagaApprovmentEndPoints;
using Application.SagaRejectionEndPoints;
using Infrastructure;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Oakton.Resources;
using Persons;
using Persons.Contracts;
using Persons.Read.GetPersonWithSum;
using Persons.Write.CreatePersonUseCase;
using Persons.Write.Subscriptions;
using Shared;
using Shared.Infrastructure;
using Weasel.Core;
using Wolverine;
using Wolverine.Http;
using Wolverine.Kafka;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
services.AddScoped<IRepository, Repository>();

// добавляем сервисы из PersonModule
services.AddPersonServices();

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new ArgumentNullException("Строка подключения к БД отсутствует");
var martenConfig = configuration.GetSection("EventStore").Get<MartenSettings>()
        ?? throw new ArgumentNullException("Настройки EventStore отсутствуют"); ;

// добавляем поддерку MartenDB
services.AddMarten(options =>
{
    options.Connection(connectionString);
    options.AutoCreateSchemaObjects = AutoCreate.All;
    options.UseNewtonsoftForSerialization(nonPublicMembersStorage: NonPublicMembersStorage.All);
    options.Events.DatabaseSchemaName = martenConfig.WriteSchema;
    options.Events.StreamIdentity = Marten.Events.StreamIdentity.AsString;

    if (!string.IsNullOrEmpty(martenConfig.ReadSchema))
        options.DatabaseSchemaName = martenConfig.ReadSchema;

    // регистрируем проекцию.
    options.Projections.Add<PersonWithSumProjection>(ProjectionLifecycle.Async);
})
.IntegrateWithWolverine()
.UseLightweightSessions()
// добавляем пописку на сообщения
.AddSubscriptionWithServices<PersonApprovedToKafkaSubscription>(ServiceLifetime.Singleton, o =>
{
    // Обрабатывать не более 10 сообщений за раз.
    o.Options.BatchSize = 10;
})
.AddAsyncDaemon(DaemonMode.HotCold);

builder.Services.AddControllers();

// Добавляем 
builder.Host.UseWolverine(opts =>
{
    opts.Policies.AutoApplyTransactions();
    //подключаемся к серверу кафки.
    opts.UseKafka("kafka:9092");

    //Будем публиковать в кафке ниже приведённые события.
    opts.PublishMessage<PersonApproved>().ToKafkaTopic("CreatePersonUseCase.PersonApproved");
    opts.PublishMessage<PersonRejected>().ToKafkaTopic("CreatePersonUseCase.PersonRejected");
    //Это событие будет положено в кафку в классе AccountApprovedToKafkaSubscription.
    opts.PublishMessage<PersonApprovedIntegrationEvent>().ToKafkaTopic("PersonApprovedIntegrationEvent");

    //Будем получать из топиков кафки следующие события.
    opts.ListenToKafkaTopic("CreatePersonUseCase.PersonApproved");
    opts.ListenToKafkaTopic("CreatePersonUseCase.PersonRejected");

    opts.Services.AddResourceSetupOnStartup();

    //подключаем эндпоинты из библиотек.
    opts.Discovery.IncludeAssembly(typeof(CreatePersonEndPoint).Assembly);
    opts.Discovery.IncludeAssembly(typeof(RejectEndPoints).Assembly);
    opts.Discovery.IncludeAssembly(typeof(ApproveEndPoints).Assembly);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//отображаем запросы на эндпоинты подключённые ранее из Application.
app.MapWolverineEndpoints();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
