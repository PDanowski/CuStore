using CuStore.WebUI.Api;

namespace CuStore.WebUI.Models;

public class StoreIndexViewModel
{
    public IEnumerable<ProductSummaryResponse> Products { get; set; } = [];
    public int? CategoryId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
}
