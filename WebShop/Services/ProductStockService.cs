using Database;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Services.Interfaces;

namespace WebShop.Services
{

    public class OutOfStockException : Exception
    {

        public OutOfStockException(int productOutOfStock, string message)
            : base(message)
        {
            ProductsOutOfStock = productOutOfStock;
        }

        public OutOfStockException(int productOutOfStock,
            string message,
            Exception innerException)
            : base(message, innerException)
        {
            ProductsOutOfStock = productOutOfStock;
        }

        public int ProductsOutOfStock;
    }
    public class ProductStockService : IProductStockService
    {
        private readonly ApplicationContext _context;

        public ProductStockService(ApplicationContext context)
        {
            _context = context;
        }
        public bool IsInStock(List<CartItem> cartItems, out List<int> ProductsOutOfStock)
        {
            ProductsOutOfStock = new();
            foreach (var i in cartItems)
            {
                if (IsInStock(i.Product, i.Quantity) == false)
                    ProductsOutOfStock.Add(i.Id);
            }

            if (ProductsOutOfStock.Count > 0)
                return false;

            return true;
        }

        public bool IsInStock(Product product, int quantity)
        {
            if (product.InStock - quantity > 1)
                return true;

            return false;
        }
        public bool DecQuantity(Product product, int count)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (product.InStock - count < 0)
            {
                return false;
            }

            product.InStock -= count;

            return true;
        }

        public bool IncQuantity(Product product, int count)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.InStock += count;

            return true;
        }

        public bool UpdateQuantity(Product product, int newQuantity)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (newQuantity <= 0)
            {
                return false;
            }

            product.InStock = newQuantity;

            return true;
        }

        public Task ReturnProductsToStock(List<OrderItem> orderItems)
        {
            foreach (var i in orderItems)
            {
                IncQuantity(i.Product, i.Quantity);
            }

            return _context.SaveChangesAsync();
        }

        public async Task GetProductsFromStock(List<CartItem> cartItems)
        {

            foreach (var i in cartItems)
            {
                if (DecQuantity(i.Product, i.Quantity) == false)
                {
                    // sets cart item's product to unchanged state
                    // to avoid next call 'SaveChanges'
                    foreach (var j in cartItems)
                    {
                        _context.Entry(j.Product).State = EntityState.Unchanged;
                    }
                    throw new OutOfStockException(i.Product.Id, "Product out of stock");
                }
            }

            bool saved = false;
            while (!saved)
            {
                try
                {

                    await _context.SaveChangesAsync();
                    saved = true;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Product)
                        {
                            var currentValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            if (databaseValues == null)
                            {
                                throw
                                    new OrderServiceException(
                                        "Product removed from the database");
                            }

                            var currentValueInStock = (int)currentValues["InStock"];
                            var databaseValueInStock = (int)databaseValues["InStock"];
                            var originalValueInStock = (int)entry.OriginalValues["InStock"];

                            if (databaseValueInStock - (originalValueInStock - currentValueInStock) < 0)
                            {
                                throw new OutOfStockException((int)databaseValues["Id"], "Product out of stock");
                            }

                            currentValues["InStock"] =
                                databaseValueInStock - (originalValueInStock - currentValueInStock);

                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }
        }
    }
}
