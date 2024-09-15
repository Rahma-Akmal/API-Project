using API_Project.IRepo;
using API_Project.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepo _paymentRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<APPUser> _userManager;


        public PaymentController(IPaymentRepo paymentRepository, IHttpContextAccessor _contextAccessor, UserManager<APPUser> userManager)
        {
            _paymentRepository = paymentRepository;
            this._contextAccessor = _contextAccessor;
            this._userManager = userManager;
        }
        [HttpPost]
        public IActionResult process(Payment payment)
        {
            var user =  _userManager.GetUserId(User);
            if (user == null) 
            { 
                return NotFound();
            }
            payment.UserId = user;

            var processedPayment = _paymentRepository.ProcessPayment(payment);
            return Ok(processedPayment);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            var payment=_paymentRepository.GetPaymentById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
    }
}
