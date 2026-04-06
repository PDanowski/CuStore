using CuStore.Crm;
using CuStore.Core.Abstractions;

namespace CuStore.WebApi.Services;

public class GrpcCrmClient(CrmApi.CrmApiClient client) : ICrmClient
{
    public async Task<Guid> CreateCustomerDataAsync(string customerCode, int bonusPoints, CancellationToken cancellationToken = default)
    {
        var reply = await client.AddCustomerAsync(new AddCustomerRequest
        {
            ExternalCode = customerCode,
            BonusPoints = bonusPoints,
        }, cancellationToken: cancellationToken);

        return Guid.Parse(reply.CustomerId);
    }

    public async Task<bool> AddPointsForCustomerAsync(Guid customerGuid, int points, CancellationToken cancellationToken = default)
    {
        var reply = await client.AddPointForCustomerAsync(new AddPointForCustomerRequest
        {
            CustomerId = customerGuid.ToString(),
            Points = points,
        }, cancellationToken: cancellationToken);

        return reply.Success;
    }

    public async Task<bool> RemoveCustomerDataAsync(Guid customerGuid, CancellationToken cancellationToken = default)
    {
        var reply = await client.RemoveCustomerAsync(new RemoveCustomerRequest
        {
            CustomerId = customerGuid.ToString(),
        }, cancellationToken: cancellationToken);

        return reply.Success;
    }

    public async Task<int> GetPointsForCustomerAsync(Guid customerGuid, CancellationToken cancellationToken = default)
    {
        var reply = await client.GetPointsAsync(new GetPointsRequest
        {
            CustomerId = customerGuid.ToString(),
        }, cancellationToken: cancellationToken);

        return reply.Points;
    }
}
