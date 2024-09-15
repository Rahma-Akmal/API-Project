using API_Project.Data;
using API_Project.IRepo;
using API_Project.Model;
using Microsoft.AspNetCore.Identity;
using System;

namespace API_Project.Repo
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<APPUser> _userManager;

        public PaymentRepo(DataContext context, IHttpContextAccessor contextAccessor, UserManager<APPUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public Payment GetPaymentById(int id)
        {
            return _context.Payments.Find(id);
        }

        public Payment ProcessPayment(Payment payment)
        {
           _context.Payments.Add(payment);
            _context.SaveChanges();
            return payment;
        }
    }
}
