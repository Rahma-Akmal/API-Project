using API_Project.Data;
using API_Project.IRepo;
using API_Project.Model;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Repo
{
    public class CartRepo : ICartRepo
    {
        private readonly DataContext _dataContext;
        public CartRepo(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void AddToCart(Cart cart)
        {
            _dataContext.Carts.Add(cart);
            _dataContext.SaveChanges();
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart=GetById(userId);
            var item=cart.Items.FirstOrDefault(x => x.ProductId == productId);
            cart.Items.Remove(item);
            UpdateCart(cart);
        }

        public Cart GetById(string id)
        {
            return _dataContext.Carts.Include(s=>s.Items).ThenInclude(a=>a.Product).SingleOrDefault(x=>x.UserId==id);
        }

        public void UpdateCart(Cart cart)
        {
            _dataContext.Update(cart);
            _dataContext.SaveChanges();
        }
    }
}
