using Shared.Abstractions;

namespace Shared.Infrastructure;

/// <summary>
/// Репозиторий.
/// </summary>
public interface IRepository
{
    /// <summary>
    /// Сохраняем агрегат
    /// </summary>
    /// <param name="aggregate"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task StoreAsync(Aggregate aggregate, CancellationToken ct = default);

    /// <summary>
    /// Восстанавливаем состояние агрегата по его событиям.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <param name="version"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<T> LoadAsync<T>(string id, int? version = null, CancellationToken ct = default) where T : Aggregate;
}
