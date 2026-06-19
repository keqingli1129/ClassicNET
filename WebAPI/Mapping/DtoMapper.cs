using System.Collections.Generic;
using System.Linq;
using Shared.Contracts.DTOs;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    /// <summary>
    /// Extension methods that map EF entity models to shared DTO types.
    /// </summary>
    public static class DtoMapper
    {
        /// <summary>
        /// Maps a <see cref="Category"/> entity to a <see cref="CategoryDto"/>.
        /// </summary>
        public static CategoryDto ToDto(this Category category)
        {
            if (category == null)
            {
                return null;
            }

            return new CategoryDto
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Picture = category.Picture,
                Products = category.Products?.Select(p => p.ToDto()).ToList()
            };
        }

        /// <summary>
        /// Maps a collection of <see cref="Category"/> entities to a list of <see cref="CategoryDto"/>.
        /// </summary>
        public static List<CategoryDto> ToDtoList(this IEnumerable<Category> categories)
        {
            if (categories == null)
            {
                return new List<CategoryDto>();
            }

            return categories.Select(c => c.ToDto()).ToList();
        }

        /// <summary>
        /// Maps a <see cref="Product"/> entity to a <see cref="ProductDto"/>.
        /// </summary>
        public static ProductDto ToDto(this Product product)
        {
            if (product == null)
            {
                return null;
            }

            return new ProductDto
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                SupplierID = product.SupplierID,
                CategoryID = product.CategoryID,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued
            };
        }
    }
}
