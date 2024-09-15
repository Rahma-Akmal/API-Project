using API_Project.Model;

namespace API_Project.IRepo
{
    public interface IOrderRepo
    {
        public IEnumerable<Order> GetAll();
        public Order GetById(int id,string userid);
        public void AddOrder(Order order);
        public void UpdateOrder(Order order);
        public void DeleteOrder(int id);
    }
}
