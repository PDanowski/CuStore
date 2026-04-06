using CuStore.Core.Abstractions;
using CuStore.Core.Entities;
using CuStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CuStore.Infrastructure.Repositories;

public class CategoryRepository(StoreDbContext context) : ICategoryRepository
{
    public IEnumerable<Category> GetCategories() =>
        context.Categories.Include(c => c.ParentCategory).ToList();

    public IEnumerable<Category> GetParentCategories() =>
        context.Categories.Where(c => c.ParentCategoryId == null).ToList();

    public bool SaveCategory(Category category)
    {
        var existingCategory = context.Categories.SingleOrDefault(c => c.Id == category.Id);
        if (existingCategory is null)
        {
            return false;
        }

        context.Entry(existingCategory).CurrentValues.SetValues(category);
        context.SaveChanges();
        return true;
    }

    public bool AddCategory(Category category)
    {
        context.Categories.Add(category);
        context.SaveChanges();
        return true;
    }

    public bool RemoveCategory(int categoryId)
    {
        var category = context.Categories.SingleOrDefault(c => c.Id == categoryId);
        if (category is null)
        {
            return false;
        }

        context.Categories.Remove(category);
        context.SaveChanges();
        return true;
    }

    public Category? GetCategoryById(int id) =>
        context.Categories.Include(c => c.ParentCategory).FirstOrDefault(c => c.Id == id);
}
