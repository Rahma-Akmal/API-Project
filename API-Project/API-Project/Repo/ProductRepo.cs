using API_Project.Data;
using API_Project.DTO;
using API_Project.IRepo;
using API_Project.Model;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Repo
{
    public class ProductRepo : IProductrepo
    {
        private readonly DataContext _dataContext;
        public ProductRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void AddProduct(ProductDTO pro)
        {
            Product product = new Product();
            product.ProductName=pro.ProductName;
            product.Price =pro.Price;
            product.Description =pro.Description;
            product.CategoryId =pro.CategoryId;
            _dataContext.Products.Add(product);
            _dataContext.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _dataContext.Products.FirstOrDefault(x=>x.Id==id);
            if (product != null)
            {
                _dataContext.Products.Remove(product);
                _dataContext.SaveChanges();
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return _dataContext.Products.Include(x => x.Category).ToList();
        }

        public Product GetById(int id)
        {
            return _dataContext.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
        }

        public void UpdateProduct(ProductDTO pro, int Id)
        {
            var Product = GetById(Id);
            if (Product != null)
            {
                Product.ProductName= pro.ProductName;
                Product.Price = pro.Price;
                Product.Description = pro.Description;
                Product.CategoryId = pro.CategoryId;
                _dataContext.Products.Update(Product);
                _dataContext.SaveChanges();
            }
        }
    }
}
