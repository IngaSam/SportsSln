﻿using System.Linq;
using System.Security.Cryptography;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        //Добавление элемента в корзину
        public void Can_Add_New_Lines()
        {
            //Организация - создание нескольких тестовых товаров
            Product p1 = new Product { ProductID = 1, Name = "P1"};
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            //Организация - создание новой корзины
            Cart target = new Cart();
            //Действие 
            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            CartLine[] results=target.Lines.ToArray();
            //Утверждение
            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        //В ранее добавленном объекте увеличение количества экземпляров, не создание нового. 
        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Организация - создание нескольких тестовых товаров
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            //Организация - создание новой корзины
            Cart target = new Cart();

            //Действие
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[]results = target.Lines.OrderBy(c=>c.Product.ProductID).ToArray();
            //Утверждение
            Assert.Equal(2,results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        //Возможность менять решение - удаление товаров из корзины
        [Fact]
        public void Can_Remove_Line()
        {
            //Организация - создание нескольких тестовых товаров
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            //Организация - создание новой корзины
            Cart target = new Cart();
            //Организация - добавление нескольких элементов в корзину
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);
            //Действие 
            target.RemoveLine(p2);
            //Утверждение
            Assert.Empty(target.Lines.Where(c=>c.Product==p2));
            Assert.Equal(2,target.Lines.Count());
        }

        //Вычисление общей стоимости элементов в корзине
        [Fact]
        public void Calculate_Cart_Total()
        {
            //Организация - создание нескольких тестовых товаров
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            //Организация - создание новой корзины
            Cart target = new Cart();

            //Действие
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();
            //Утверждение
            Assert.Equal(450M, result);
        }

        //Проверка что после очистки корзины ее содержимое корректно удаляется
        [Fact]
        public void Can_Clear_Contents()
        {
            //Организация - создание нескольких тестовых товаров
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            //Организация - создание новой корзины
            Cart target = new Cart();

            //Организация - добавление нескольких элементов в корзину
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            
            
            //Действие - очистка корзины
            target.Clear();

            //Утверждение
            Assert.Empty(target.Lines);
        }

    }
}
