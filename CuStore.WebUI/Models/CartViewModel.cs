using CuStore.WebUI.Api;

namespace CuStore.WebUI.Models;

public class CartViewModel
{
    public List<CartDisplayItem> Items { get; set; } = [];
    public decimal Subtotal { get; set; }
    public IEnumerable<ShippingMethodResponse> ShippingMethods { get; set; } = [];
    public int SelectedShippingMethodId { get; set; }
    public decimal ShippingPrice { get; set; }
    public decimal Total { get; set; }
}

public class CartDisplayItem
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal LineTotal => Quantity * Price;
}
