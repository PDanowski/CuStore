using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Abstract
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<Category> GetParentCategories();
        bool SaveCategory(Category category);
        bool AddCategory(Category category);
        bool RemoveCategory(int categoryId);
        Category GetCategoryById(int id);
    }
}
