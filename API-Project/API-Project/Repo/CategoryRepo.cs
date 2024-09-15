using API_Project.Data;
using API_Project.DTO;
using API_Project.IRepo;
using API_Project.Model;

namespace API_Project.Repo
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly DataContext _dataContext;
        public CategoryRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void AddCategory(Category category)
        {
            _dataContext.Categories.Add(category);
            _dataContext.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
           var category =GetById(id);
            _dataContext.Categories.Remove(category);
            _dataContext.SaveChanges();
        }

        public IEnumerable<Category> GetAll()
        {
            return _dataContext.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return _dataContext.Categories.FirstOrDefault(s => s.Id == id);
        }

        public void UpdateCategory(CategoryDTO category, int Id)
        {
            var cat = GetById(Id);
            cat.Name = category.Name;
           _dataContext.Categories.Update(cat);
            _dataContext.SaveChanges();
        }
    }
}
