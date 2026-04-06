namespace CuStore.Api.Contracts;

public sealed record AddToCartRequest(int ProductId, int Quantity = 1);
public sealed record CheckoutRequest(int ShippingMethodId, bool UseUserAddress, string? FirstName, string? LastName, string? Phone, string? Street, string? City, string? Country, string? PostalCode);
public sealed record RegisterRequest(string UserName, string Email, string FirstName, string LastName, DateTime BirthDate);
