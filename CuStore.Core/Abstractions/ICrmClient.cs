namespace CuStore.Core.Abstractions;

public interface ICrmClient
{
    Task<Guid> CreateCustomerDataAsync(string customerCode, int bonusPoints, CancellationToken cancellationToken = default);
    Task<bool> AddPointsForCustomerAsync(Guid customerGuid, int points, CancellationToken cancellationToken = default);
    Task<bool> RemoveCustomerDataAsync(Guid customerGuid, CancellationToken cancellationToken = default);
    Task<int> GetPointsForCustomerAsync(Guid customerGuid, CancellationToken cancellationToken = default);
}
