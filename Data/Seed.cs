using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyTelegramBot.Models.Telegram;
using Newtonsoft.Json;

namespace MyTelegramBot.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context) => _context = context;
        public void SeedProducts() 
        {
            var categoriesData = System.IO.File.ReadAllText("Data/json/CategoriesSeedData.json");
            var categories = JsonConvert.DeserializeObject<List<Category>>(categoriesData);
            var oldCategories = _context.Categories;
            var EqualityCompareCategory = new EqualityCompareCategory();
            var different = categories.Except(oldCategories, EqualityCompareCategory);
            _context.AddRange(different);
            _context.SaveChanges();

            var productsData = System.IO.File.ReadAllText("Data/json/ProductsSeedData.json");
            var products = JsonConvert.DeserializeObject<List<Product>>(productsData);
            var oldProducts = _context.Products;
            var EqualityProduct = new EqualityCompareProduct();
            var diffProducts = products.Except(oldProducts, EqualityProduct);
            _context.AddRange(diffProducts);
            _context.SaveChanges();
        }
    }

    public class EqualityCompareCategory : IEqualityComparer<Category>
    {
        public bool Equals(Category c1, Category c2)
        {
            return c1.Id == c2.Id ||
                (c1.LanguageCode == c2.LanguageCode &&
                c1.Name == c2.Name &&
                c1.Products == c2.Products);
        }

        public int GetHashCode(Category obj)
        {
            unchecked
            {
                if (obj == null)
                    return 0;
                int hashCode = obj.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Id.GetHashCode();
                return hashCode;
            }
        }
    }

    public class EqualityCompareProduct : IEqualityComparer<Product>
    {
        public bool Equals(Product c1, Product c2)
        {
            return c1.Id == c2.Id || (
                c1.LanguageCode == c2.LanguageCode &&
                c1.Name == c2.Name &&
                c1.Description == c2.Description &&
                c1.Price == c2.Price &&
                c1.CategoryId == c2.CategoryId);
        }

        public int GetHashCode(Product obj)
        {
            unchecked
            {
                if (obj == null)
                    return 0;
                int hashCode = obj.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Id.GetHashCode();
                return hashCode;
            }
        }
    }
}