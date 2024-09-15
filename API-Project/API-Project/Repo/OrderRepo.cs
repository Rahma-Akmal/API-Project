using API_Project.Data;
using API_Project.IRepo;
using API_Project.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Repo
{
    public class OrderRepo : IOrderRepo
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<APPUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderRepo(DataContext dataContext,UserManager<APPUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public void AddOrder(Order order)
        {
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            order.UserId = userId;
            order.OrderDate = DateTime.Now;

            _dataContext.Orders.Add(order);
            _dataContext.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            var order =  GetById(id, userId);
            if (order != null)
            {
                _dataContext.Orders.Remove(order);
                _dataContext.SaveChanges();
            }
        }

        public IEnumerable<Order> GetAll()
        {
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            return _dataContext.Orders.Where(o => o.UserId == userId).ToList();
        }

        public Order GetById(int id,string userid)
        {
            return _dataContext.Orders.FirstOrDefault(x => x.Id == id && x.UserId == userid);
        }

        public void UpdateOrder(Order order)
        {
            var existingOrder = GetById(order.Id, order.UserId);
            if (existingOrder != null)
            {
                _dataContext.Entry(existingOrder).CurrentValues.SetValues(order);
                _dataContext.SaveChanges();
            }
        }
    }
}
