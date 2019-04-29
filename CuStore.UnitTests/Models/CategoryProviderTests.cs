using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuStore.UnitTests.Models
{
    [TestClass]
    public class CategoryProviderTests
    {
        [TestMethod]
        public void Create_Categories_SelectList()
        {
            var categories = new List<Category>()
            {
                new Category {Id = 1, Name = "Category1", ParentCategoryId = null},
                new Category {Id = 2, Name = "Category2", ParentCategoryId = 1},
                new Category {Id = 3, Name = "Category3", ParentCategoryId = 1},
                new Category {Id = 4, Name = "Category4", ParentCategoryId = 1},
                new Category {Id = 5, Name = "Category5", ParentCategoryId = 1}
            };

            var result = CategroriesProvider.CreateSelectList(categories);

            Assert.AreEqual(result[0].Text, "- Category1");
            Assert.AreEqual(result[1].Text, "--> Category2");
            Assert.AreEqual(result[2].Text, "--> Category3");
            Assert.AreEqual(result[3].Text, "--> Category4");
            Assert.AreEqual(result[4].Text, "--> Category5");
        }

        [TestMethod]
        public void CantCreate_Categories_SelectList()
        {
            var result = CategroriesProvider.CreateSelectList(new List<Category>());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);
        }
    }
}
