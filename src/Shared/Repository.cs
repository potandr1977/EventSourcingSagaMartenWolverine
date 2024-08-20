using Marten;
using Shared.Abstractions;
using Shared.Infrastructure;

namespace Shared;

/// <inheritdoc/>
public sealed class Repository(IDocumentStore store) : IRepository
{
    //Marten document store
    private readonly IDocumentStore store = store;

    /// Получаем несохранённые события из агрегата и сохраняем их.
    public async Task StoreAsync(Aggregate aggregate, CancellationToken ct = default)
    {
        // получаем сессию для работы с событиями.
        await using var session = await store.LightweightSerializableSessionAsync(token: ct);
        // получаем список несохранённых событий из агрегата
        var events = aggregate.GetUncommittedEvents().ToArray();
        // добавляем события в стрим с идентификатором aggregate.Id
        session.Events.Append(aggregate.Id, aggregate.Version, events);

        // сохраняем изменения.
        await session.SaveChangesAsync(ct);
        // очищаем список несохранённых событий.
        aggregate.ClearUncommittedEvents();
    }

    /// Восстанавливаем состояние агрегата по событиям.
    public async Task<T> LoadAsync<T>(
        string id,
        int? version = null,
        CancellationToken ct = default
    ) where T : Aggregate
    {
        // получаем сессию для работы с событиями.
        await using var session = await store.LightweightSerializableSessionAsync(token: ct);
        // восстанавливаем состояние агрегата, читая из бд события стрима агрегата.
        // при этом Marten вызовет методы Apply для каждого из сохранённых событий.

        var stream = await session.Events.FetchForWriting<T>(id);

        return stream.Aggregate;        
    }
}