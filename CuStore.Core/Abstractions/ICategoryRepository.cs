using CuStore.Core.Entities;

namespace CuStore.Core.Abstractions;

public interface ICategoryRepository
{
    IEnumerable<Category> GetCategories();
    IEnumerable<Category> GetParentCategories();
    bool SaveCategory(Category category);
    bool AddCategory(Category category);
    bool RemoveCategory(int categoryId);
    Category? GetCategoryById(int id);
}
