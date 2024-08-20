using Marten;
using Marten.Events.Daemon;
using Marten.Events.Daemon.Internals;
using Marten.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Persons.Contracts;
using Wolverine;

namespace Persons.Write.Subscriptions;

// Класс интеграционного события, которые мы будем публиковать в кафку.
public record PersonApprovedIntegrationEvent(string Key, string Value);

/// <summary>
/// Подписка на события типа PersonApproved. 
/// После получения доменное событие конвертируется в интеграционное, а интеграционное отправляется в Кафку.
/// </summary>
public class PersonApprovedToKafkaSubscription : SubscriptionBase
{
    private readonly IServiceProvider _serviceProvider;

    public PersonApprovedToKafkaSubscription(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        SubscriptionName = nameof(PersonApprovedToKafkaSubscription);

        // Подписываемся только на события типа PersonApproved
        IncludeType<PersonApproved>();

        // настраиваем сколько событий демон будет извлекать за раз
        // и сколько будет держать в памяти.
        Options.BatchSize = 1000;
        Options.MaximumHopperSize = 10000;

        // Позиция с которой читаем события (с текущего события)
        Options.SubscribeFromPresent();
    }

    /// <summary>
    /// Обрабатываем события.
    /// </summary>
    public override async Task<IChangeListener> ProcessEventsAsync(
        EventRange page,
        ISubscriptionController controller,
        IDocumentOperations operations,
        CancellationToken cancellationToken)
    {
        // с помощью Woverine будем отправлять интеграционные события в кафку.
        var messageBus = _serviceProvider.GetService<IMessageBus>() ?? throw new ArgumentNullException("Шина событий не зарегистрирована в IoC");

        foreach (var @event in page.Events)
        {
            await messageBus.PublishAsync(
                new PersonApprovedIntegrationEvent(@event.Data.GetType().Name, JsonConvert.SerializeObject(@event.Data)));
        }

        return NullChangeListener.Instance;
    }
}