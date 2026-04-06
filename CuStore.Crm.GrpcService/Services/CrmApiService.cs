using CuStore.Crm;
using CuStore.Infrastructure.Crm;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace CuStore.Crm.GrpcService.Services;

public class CrmApiService(CrmDbContext dbContext) : CrmApi.CrmApiBase
{
    public override async Task<CustomerDataReply> GetCustomerData(GetCustomerDataRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.CustomerId, out var customerId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer id."));
        }

        var customerData = await dbContext.CustomerData.FirstOrDefaultAsync(cd => cd.Id == customerId, context.CancellationToken);
        if (customerData is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Can't find customer with GUID: {customerId}"));
        }

        return new CustomerDataReply
        {
            Id = customerData.Id.ToString(),
            ExternalCode = customerData.ExternalCode,
            Points = customerData.Points,
            Ratio = (double)customerData.Ratio,
        };
    }

    public override async Task<GetPointsReply> GetPoints(GetPointsRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.CustomerId, out var customerId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer id."));
        }

        var customerData = await dbContext.CustomerData.FirstOrDefaultAsync(cd => cd.Id == customerId, context.CancellationToken);
        if (customerData is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Can't find customer with GUID: {customerId}"));
        }

        return new GetPointsReply { Points = customerData.Points };
    }

    public override async Task<AddCustomerReply> AddCustomer(AddCustomerRequest request, ServerCallContext context)
    {
        var customer = new CrmCustomer
        {
            Id = Guid.NewGuid(),
            ExternalCode = request.ExternalCode,
            Points = request.BonusPoints,
            Ratio = 1.0m,
        };

        dbContext.CustomerData.Add(customer);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        return new AddCustomerReply { CustomerId = customer.Id.ToString() };
    }

    public override async Task<BoolReply> AddCustomerData(AddCustomerDataRequest request, ServerCallContext context)
    {
        var id = Guid.TryParse(request.Id, out var customerId) ? customerId : Guid.NewGuid();

        dbContext.CustomerData.Add(new CrmCustomer
        {
            Id = id,
            ExternalCode = request.ExternalCode,
            Points = request.Points,
            Ratio = (decimal)request.Ratio,
        });

        await dbContext.SaveChangesAsync(context.CancellationToken);
        return new BoolReply { Success = true };
    }

    public override async Task<BoolReply> RemoveCustomer(RemoveCustomerRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.CustomerId, out var customerId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer id."));
        }

        var customerData = await dbContext.CustomerData.FirstOrDefaultAsync(cd => cd.Id == customerId, context.CancellationToken);
        if (customerData is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Can't find customer with GUID: {customerId}"));
        }

        dbContext.CustomerData.Remove(customerData);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        return new BoolReply { Success = true };
    }

    public override async Task<BoolReply> AddPointForCustomer(AddPointForCustomerRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.CustomerId, out var customerId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid customer id."));
        }

        var customerData = await dbContext.CustomerData.FirstOrDefaultAsync(cd => cd.Id == customerId, context.CancellationToken);
        if (customerData is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Can't find customer with GUID: {customerId}"));
        }

        customerData.Points = request.Points;
        await dbContext.SaveChangesAsync(context.CancellationToken);
        return new BoolReply { Success = true };
    }
}
