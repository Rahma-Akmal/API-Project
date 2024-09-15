using API_Project.DTO;
using API_Project.Model;

namespace API_Project.IRepo
{
    public interface IWishListRepo
    {
        public void Add(WishList wishList);
        public WishList GetByIdandUserId(string userId, int productid);
        public void Update(WishList wish);
        public WishList GetByUserId(string userId);
    }
}
