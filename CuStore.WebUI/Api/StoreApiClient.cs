using System.Net.Http.Json;

namespace CuStore.WebUI.Api;

public class StoreApiClient(HttpClient httpClient)
{
    public async Task<ProductListResponse> GetProductsAsync(int? categoryId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = $"/api/products?categoryId={categoryId}&pageNumber={pageNumber}&pageSize={pageSize}";
        return await httpClient.GetFromJsonAsync<ProductListResponse>(query, cancellationToken)
            ?? new ProductListResponse([], 0, pageNumber, pageSize);
    }

    public async Task<IReadOnlyList<ShippingMethodResponse>> GetShippingMethodsAsync(CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<IReadOnlyList<ShippingMethodResponse>>("/api/shippingmethods", cancellationToken)
            ?? [];
    }

    public async Task<CartResponse> GetCartAsync(string userId, CancellationToken cancellationToken)
    {
        return await httpClient.GetFromJsonAsync<CartResponse>($"/api/users/{userId}/cart", cancellationToken)
            ?? new CartResponse([], 0m);
    }

    public async Task AddToCartAsync(string userId, int productId, int quantity, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(
            $"/api/users/{userId}/cart/items",
            new AddToCartRequest(productId, Math.Max(1, quantity)),
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveFromCartAsync(string userId, int productId, CancellationToken cancellationToken)
    {
        var response = await httpClient.DeleteAsync($"/api/users/{userId}/cart/items/{productId}", cancellationToken);
        if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.NotFound)
        {
            response.EnsureSuccessStatusCode();
        }
    }

    public async Task EnsureDemoUserAsync(string userId, CancellationToken cancellationToken)
    {
        var request = new RegisterRequest(
            UserName: "demo@custore.local",
            Email: "demo@custore.local",
            FirstName: "Demo",
            LastName: "User",
            BirthDate: new DateTime(1990, 1, 1));

        var response = await httpClient.PostAsJsonAsync($"/api/users/{userId}/register", request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<CheckoutResponse?> CheckoutAsync(string userId, int shippingMethodId, CancellationToken cancellationToken)
    {
        var request = new CheckoutRequest(
            shippingMethodId,
            UseUserAddress: true,
            FirstName: null,
            LastName: null,
            Phone: null,
            Street: null,
            City: null,
            Country: null,
            PostalCode: null);

        var response = await httpClient.PostAsJsonAsync($"/api/users/{userId}/checkout", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<CheckoutResponse>(cancellationToken);
    }
}
