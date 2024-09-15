using API_Project.Data;
using API_Project.DTO;
using API_Project.IRepo;
using API_Project.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace API_Project.Repo
{
    public class WishListRepo : IWishListRepo
    {
        private readonly DataContext context;
        public WishListRepo(DataContext context)
        {
            this.context = context;

        }

        public void Add(WishList wishList)
        {
            context.WishLists.Add(wishList);
            context.SaveChanges();
        }

        public WishList GetByIdandUserId(string userId, int productid)
        {
            return context.WishLists.Include(w => w.WishListItem).ThenInclude(i => i.Product).FirstOrDefault(u => u.UserId == userId);

        }

        public WishList GetByUserId(string userId)
        {
            return context.WishLists.Include(w => w.WishListItem).ThenInclude(i => i.Product).FirstOrDefault(w => w.UserId == userId);
        }

        public void Update(WishList wish)
        {
            context.WishLists.Update(wish);
            context.SaveChanges();
        }
    }
}
