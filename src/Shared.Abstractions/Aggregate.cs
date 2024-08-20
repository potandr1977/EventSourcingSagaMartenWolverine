using System.Text.Json.Serialization;

namespace Shared.Abstractions;
/// <summary>
/// Абстракция агрегата.
/// </summary>
public abstract class Aggregate : Entity
{
    /// <summary>
    /// Версия агрегата, должна обновляться после добавления каждого события.
    /// </summary>
    public long Version { get; set; }

    // несохранённые сообщения.
    [JsonIgnore] private readonly List<object> _uncommittedEvents = new List<object>();

    /// <summary>
    /// Получаем список несохранённых событий для сохранения.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<object> GetUncommittedEvents()
    {
        return _uncommittedEvents;
    }

    /// <summary>
    /// Чистим список незакоммиченных событий после сохранения.
    /// </summary>
    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    /// <summary>
    /// добавляем новое событие в список несохранённых.
    /// </summary>
    /// <param name="event"></param>
    protected void AddUncommittedEvent(object @event)
    {
        _uncommittedEvents.Add(@event);
    }
}
