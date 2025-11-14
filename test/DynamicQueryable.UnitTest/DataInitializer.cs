using DynamicQueryable.UnitTest.Mock;
using DynamicQueryable.UnitTest.Mock.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicQueryable.UnitTest
{
    [TestClass]
    public static class DataInitializer
    {
        public static readonly string GuidString = "62559CB0-1EEF-4256-958E-AE4B95974F4E";
        public static readonly int ProductSampleCount = 1000;
        public static readonly int ProductSampleExtraCount = 5;
        public static readonly int LoopProductSampleCount = ProductSampleCount - ProductSampleExtraCount;
        public static int LoopProductSampleCountDiv0;
        public static int LoopProductSampleCountDiv1;
        public static readonly int DefaultToTakeCount = 10;
        //[AssemblyInitialize()] 
        public static void InitializeSeed(DynamicQueryableDbContext dbContext)
        {
            //using (DynamicQueryableContext context = new DynamicQueryableContext())
            //{
            if (dbContext.Product.Any())
            {
                return;
            }

            var random = new Random();
            var fourthCategory = new ProductCategory
            {
                Name = "fourth"
            };
            var thirdCategory = new ProductCategory
            {
                Name = "third",
                ParentProductCategory = fourthCategory
            };
            var secondCategory = new ProductCategory
            {
                Name = "second",
                ParentProductCategory = thirdCategory
            };
            var redCategory = new ProductCategory
            {
                Name = "red",
                ParentProductCategory = secondCategory
            };
            var blackCategory = new ProductCategory
            {
                Name = "black",
                ParentProductCategory = secondCategory
            };
            var model1 = new ProductModel
            {
                Name = "Model 1"
            };

            var div2_0 = 0;
            var div2_1 = 0;
            for (var i = 0; i < LoopProductSampleCount; i++)
            {

                if(i % 2 == 0)
                    div2_0++;
                else 
                    div2_1++;

               


                dbContext.Product.Add(new Product
                {
                    Color = i % 2 == 0 ? "red" : "black",
                    ProductCategory = i % 2 == 0 ? redCategory : blackCategory,
                    ProductModel = model1,
                    ListPrice = (decimal)(i / 5.0),
                    Name = i % 2 == 0 ? null : $"Product {i}",
                    ProductNumber = Guid.NewGuid().ToString(),
                    Rowguid = Guid.Parse(GuidString),
                    Size = i % 2 == 0 ? "L" : "M",
                    SellStartDate = DateTime.Today.AddHours(8 * i),
                    StandardCost = i + 1,
                    Weight = i % 4,
                    SalesOrderDetail = new List<SalesOrderDetail>
                        {
                            new SalesOrderDetail
                            {
                                LineTotal = i % 54,
                                OrderQty = 5,
                                UnitPrice = i + i,
                                UnitPriceDiscount = i + i / 2
                            },
                            new SalesOrderDetail
                            {
                                LineTotal = i + 15 % 64,
                                OrderQty = 3,
                                UnitPrice = i + i,
                                UnitPriceDiscount = i + i / 2
                            }
                        },
                    ProductExtension = i % 2 == 0 ? new ProductExtension { Name = "fek" } : new ProductExtension { Name = "ext", IsDeleted = true }
                });
            }

            DataInitializer.LoopProductSampleCountDiv0 = div2_0;
            DataInitializer.LoopProductSampleCountDiv1 = div2_1;

            dbContext.Product.Add(new Product
            {
                Color = "red",
                ProductCategory = redCategory,
                ProductModel = model1,
                ListPrice = 522,
                Name = $"MySpecial 457",
                ProductNumber = Guid.NewGuid().ToString(),
                Rowguid = Guid.Parse(GuidString),
                Size = "S",
                SellStartDate = DateTime.Today.AddHours(8 * 9),
                StandardCost = 1,
                Weight = 32,
                SalesOrderDetail = new List<SalesOrderDetail>
                {

                },
                ProductExtension = new ProductExtension { Name = "aaa" }
            });



            dbContext.Product.Add(new Product
            {
                Color = "red",
                ProductCategory = redCategory,
                ProductModel = model1,
                ListPrice = 522,
                Name = $"1,3-Dimethylu",
                ProductNumber = Guid.NewGuid().ToString(),
                Rowguid = Guid.Parse(GuidString),
                Size = "S",
                SellStartDate = DateTime.Today.AddHours(8 * 9),
                StandardCost = 1,
                Weight = 32,
                SalesOrderDetail = new List<SalesOrderDetail>
                {

                },
                ProductExtension = new ProductExtension { Name = "ext" }
            });
            dbContext.Product.Add(new Product
            {
                Color = "red",
                ProductCategory = redCategory,
                ProductModel = model1,
                ListPrice = 522,
                Name = $"1,4-Dimethylu",
                ProductNumber = Guid.NewGuid().ToString(),
                Rowguid = Guid.Parse(GuidString),
                Size = "S",
                SellStartDate = DateTime.Today.AddHours(8 * 9),
                StandardCost = 1,
                Weight = 32,
                SalesOrderDetail = new List<SalesOrderDetail>
                {

                },
                ProductExtension = new ProductExtension { Name = "zzz" }
            });
            dbContext.Product.Add(new Product
            {
                Color = "red",
                ProductCategory = redCategory,
                ProductModel = model1,
                ListPrice = 522,
                Name = $"2,4-Dimethylu",
                ProductNumber = Guid.NewGuid().ToString(),
                Rowguid = Guid.Parse(GuidString),
                Size = "S",
                SellStartDate = DateTime.Today.AddHours(8 * 9),
                StandardCost = 1,
                Weight = 32,
                SalesOrderDetail = new List<SalesOrderDetail>
                {

                },
                ProductExtension = new ProductExtension { Name = "ext" }
            });

            dbContext.Product.Add(new Product
            {
                Color = "green + red",
                ProductCategory = redCategory,
                ProductModel = model1,
                ListPrice = 522,
                Name = $"MySpecial reed + green",
                ProductNumber = Guid.NewGuid().ToString(),
                Rowguid = Guid.Parse(GuidString),
                Size = "S",
                SellStartDate = DateTime.Today.AddHours(8 * 9),
                StandardCost = 1,
                Weight = 32,
                SalesOrderDetail = new List<SalesOrderDetail>
                {
                    new SalesOrderDetail()
                    {
                           LineTotal = 5 + 15 % 64,
                            OrderQty = 3,
                            UnitPrice = 900,
                            UnitPriceDiscount = 500
                    }
                },
                ProductExtension = new ProductExtension { Name = "ext" },
                MyComplexClass = new List<ComplexClass>() { new ComplexClass { Value = "value", Value2 = "valuevaluevalue" } }
            });
            dbContext.SaveChanges();
            //}
        }
        public static void AddDateTimeSeeds(DynamicQueryableDbContext dbContext)
        {
            dbContext.Product.Add(new Product
            {
                Name = "TestIn2010",
                SellStartDate = new DateTime(2010, DateTime.Today.Month, DateTime.Today.Day)
            });
            dbContext.SaveChanges();
        }
    }
}
