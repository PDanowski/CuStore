namespace CuStore.WebUI.Api;

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

public sealed record AddToCartRequest(int ProductId, int Quantity);

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

public sealed record RegisterRequest(
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    DateTime BirthDate);

public sealed record CheckoutRequest(
    int ShippingMethodId,
    bool UseUserAddress,
    string? FirstName,
    string? LastName,
    string? Phone,
    string? Street,
    string? City,
    string? Country,
    string? PostalCode);

public sealed record CheckoutResponse(
    int OrderId,
    DateTime OrderDate,
    string Status,
    decimal Total);
