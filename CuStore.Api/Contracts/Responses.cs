namespace CuStore.Api.Contracts;

public sealed record ProductListResponse(
    IReadOnlyList<ProductSummaryResponse> Items,
    int Total,
    int PageNumber,
    int PageSize);

public sealed record ProductSummaryResponse(
    int Id,
    string Name,
    string Description,
    decimal Price);

public sealed record CartResponse(
    IReadOnlyList<CartItemResponse> Items,
    decimal Subtotal);

public sealed record CartItemResponse(
    int ProductId,
    string Name,
    int Quantity,
    decimal Price);

public sealed record ShippingMethodResponse(
    int Id,
    string Name,
    decimal Price,
    int MaximumPcs);

public sealed record RegisterResponse(
    string UserId,
    string UserName,
    string Email,
    Guid? CrmGuid);

public sealed record CheckoutResponse(
    int OrderId,
    DateTime OrderDate,
    string Status,
    decimal Total);
