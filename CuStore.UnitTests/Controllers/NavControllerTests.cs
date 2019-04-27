using System;
using System.Collections.Generic;
using System.Linq;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Controllers;
using CuStore.WebUI.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuStore.UnitTests.Controllers
{
    [TestClass]
    public class NavControllerTests
    {
        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<ICategoryRepository> mock = new Mock<ICategoryRepository>();
            mock.Setup(m => m.GetCategories())
                .Returns(new List<Category>
                    {
                        new Category{Id = 1, Name = "Cat1", ParentCategoryId = null},
                        new Category{Id = 2, Name = "Cat2", ParentCategoryId = 1},
                        new Category{Id = 3, Name = "Cat3", ParentCategoryId = 1},
                        new Category{Id = 4, Name = "Cat4", ParentCategoryId = 1}
                    });

            NavController controller = new NavController(mock.Object);

            Category[] result = ((CategoriesListViewModel) controller.Menu().Model).Categories.ToArray();

            Assert.AreEqual(result.Length, 4);
            Assert.AreEqual(result[0].Name, "Cat1");
        }

        [TestMethod]
        public void Can_Select_On_Null_Collection()
        {
            var collection1 = new List<Category>
            {
                new Category{Id = 1, Name = "Cat1", ParentCategoryId = null}
            };

            IEnumerable<Category> list1 = collection1.Where(c => c.Id == 100);
            var result1 = list1.Select(c => c.Id);

            Assert.IsNotNull(result1);

            var collection2 = new List<int>
            {
                4, 5
            };

            IEnumerable<int> list2 = collection2.Where(c => c == 100);
            var result2 = list2.Select(c => c == 200);

            Assert.IsNotNull(result2);
        }

        [TestMethod]
        public void Indicates_Seletected_Category()
        {
            Mock<ICategoryRepository> mock = new Mock<ICategoryRepository>();
            mock.Setup(m => m.GetCategories())
                .Returns(new List<Category>
                {
                    new Category{Id = 1, Name = "Cat1", ParentCategoryId = null},
                    new Category{Id = 2, Name = "Cat2", ParentCategoryId = 1},
                    new Category{Id = 3, Name = "Cat3", ParentCategoryId = 1},
                    new Category{Id = 4, Name = "Cat4", ParentCategoryId = 1}
                });

            NavController controller = new NavController(mock.Object);

            int selectedCategoryId = 2;

            int? result = ((CategoriesListViewModel)controller.Menu(selectedCategoryId).Model).SelectedCategoryId;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Value, selectedCategoryId);
        }
    }
}
