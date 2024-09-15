using API_Project.Model;

namespace API_Project.IRepo
{
    public interface IPaymentRepo
    {
         Payment GetPaymentById(int id);
         Payment ProcessPayment(Payment payment);
    }
}
