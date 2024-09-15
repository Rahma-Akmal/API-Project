using API_Project.Model;

namespace API_Project.IRepo
{
    public interface ICartRepo
    {
        public Cart GetById(string id);
        public void AddToCart(Cart cart);
        public void UpdateCart(Cart cart);
        public void DeleteFromCart(string userId, int productId);
    }
}
