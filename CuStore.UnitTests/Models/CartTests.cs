using System;
using System.Linq;
using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CuStore.UnitTests.Models
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Items()
        {
            int quantity = 1;
            Product product1 = new Product {Id = 1, Name = "Product1", Price = 10, CategoryId = 3};
            Product product2 = new Product {Id = 2, Name = "Product2", Price = 20, CategoryId = 3};

            Cart target = new Cart();

            target.AddProduct(product1, quantity);
            target.AddProduct(product2, quantity);

            CartItem[] result = target.CartItems.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product, product1);
            Assert.AreEqual(result[1].Product, product2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Items()
        {
            int quantity = 1;
            Product product1 = new Product { Id = 1, Name = "Product1", Price = 10, CategoryId = 3 };
            Product product2 = new Product { Id = 2, Name = "Product2", Price = 20, CategoryId = 3 };

            Cart target = new Cart();

            target.AddProduct(product1, quantity);
            target.AddProduct(product2, quantity);
            target.AddProduct(product2, 2*quantity);

            CartItem[] result = target.CartItems.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, quantity);
            Assert.AreEqual(result[1].Quantity, 3*quantity);
        }

        [TestMethod]
        public void Can_Change_Quantity_For_Existing_Items()
        {
            int quantity1 = 1;
            int quantity2 = 5;
            Product product1 = new Product { Id = 1, Name = "Product1", Price = 10, CategoryId = 3 };
            Product product2 = new Product { Id = 2, Name = "Product2", Price = 20, CategoryId = 3 };

            Cart target = new Cart();

            target.AddProduct(product1, quantity1);
            target.AddProduct(product2, quantity2);

            bool quantityChanged = target.ChangeProductQuantity(product2.Id, quantity1);
            CartItem[] result = target.CartItems.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(quantityChanged);
            Assert.AreEqual(result[1].Quantity, quantity1);
        }

        [TestMethod]
        public void Can_Remove_Item()
        {
            int quantity = 1;
            Product product1 = new Product { Id = 1, Name = "Product1", Price = 10, CategoryId = 3 };
            Product product2 = new Product { Id = 2, Name = "Product2", Price = 20, CategoryId = 3 };
            Product product3 = new Product { Id = 3, Name = "Product3", Price = 50, CategoryId = 4 };

            Cart target = new Cart();

            target.AddProduct(product1, quantity);
            target.AddProduct(product2, quantity);
            target.AddProduct(product3, quantity);

            target.RemoveProduct(product1.Id);

            CartItem[] result = target.CartItems.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product, product2);
            Assert.AreEqual(result[1].Product, product3);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            int quantity = 1;
            Product product1 = new Product { Id = 1, Name = "Product1", Price = 10, CategoryId = 3 };
            Product product2 = new Product { Id = 2, Name = "Product2", Price = 20, CategoryId = 3 };
            Product product3 = new Product { Id = 3, Name = "Product3", Price = 50, CategoryId = 4 };

            Cart target = new Cart();

            target.AddProduct(product1, quantity);
            target.AddProduct(product2, quantity);
            target.AddProduct(product3, quantity);

            decimal result = target.GetValue();

            Assert.AreEqual(result, 80);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            int quantity = 1;
            Product product1 = new Product { Id = 1, Name = "Product1", Price = 10, CategoryId = 3 };
            Product product2 = new Product { Id = 2, Name = "Product2", Price = 20, CategoryId = 3 };
            Product product3 = new Product { Id = 3, Name = "Product3", Price = 50, CategoryId = 4 };

            Cart target = new Cart();

            target.AddProduct(product1, quantity);
            target.AddProduct(product2, quantity);
            target.AddProduct(product3, quantity);

            target.Clear();

            Assert.AreEqual(target.CartItems.Count, 0);
        }
    }
}
