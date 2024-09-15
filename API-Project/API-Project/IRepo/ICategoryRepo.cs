using API_Project.DTO;
using API_Project.Model;

namespace API_Project.IRepo
{
    public interface ICategoryRepo
    {
        public IEnumerable<Category> GetAll();
        public Category GetById(int id);
        public void AddCategory(Category category);
        public void UpdateCategory(CategoryDTO category,int Id);
        public void DeleteCategory(int id);
    }
}
