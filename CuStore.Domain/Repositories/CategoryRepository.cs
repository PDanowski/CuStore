using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IStoreContext _context;

        public CategoryRepository(IStoreContext context)
        {
            _context = context;
        }   

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.Include(c => c.ParentCategory).ToList();
        }

        public IEnumerable<Category> GetParentCategories()
        {
            return _context.Categories.Where(c => !c.ParentCategoryId.HasValue).ToList();
        }

        public bool SaveCategory(Category category)
        {
            try
            {
                var existingCategory = _context.Categories
                    .SingleOrDefault(c => c.Id == category.Id);

                // Update 
                _context.Entry(existingCategory).CurrentValues.SetValues(category);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.Include(c => c.ParentCategory).FirstOrDefault(c => c.Id.Equals(id));
        }
        public bool AddCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public bool RemoveCategory(int categoryId)
        {
            try
            {
                Category category = new Category { Id = categoryId };
                _context.Entry(category).State = EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

    }
}
