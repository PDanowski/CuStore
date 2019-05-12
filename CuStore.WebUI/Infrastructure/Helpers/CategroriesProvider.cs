using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CuStore.Domain.Entities;

namespace CuStore.WebUI.Infrastructure.Helpers
{
    public class CategroriesProvider
    {
        public static List<SelectListItem> CreateSelectList(List<Category> categories)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            var parents = categories.Where(c => !c.ParentCategoryId.HasValue);

            foreach (var parent in parents)
            {
                selectList.Add(new SelectListItem
                {
                    Text = "- " + parent.Name,
                    Value = parent.Id.ToString()
                });

                var children = categories.Where(c => c.ParentCategoryId.HasValue && c.ParentCategoryId.Equals(parent.Id));

                foreach (var child in children)
                {
                    selectList.Add(new SelectListItem
                    {
                        Text = "--> " + child.Name,
                        Value = child.Id.ToString() 
                    });
                }
            }

            return selectList;
        }

        public static List<SelectListItem> CreateFlatSelectList(List<Category> categories)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            foreach (var category in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }

            return selectList;
        }
    }
}