using API_Project.DTO;
using API_Project.Model;

namespace API_Project.IRepo
{
    public interface IProductrepo
    {
        public IEnumerable<Product> GetAll();
        public Product GetById(int id);
        public void AddProduct(ProductDTO product);
        public void UpdateProduct(ProductDTO product, int Id);
        public void DeleteProduct(int id);
    }
}
