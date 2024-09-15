using API_Project.DTO;
using API_Project.IRepo;
using API_Project.Model;
using API_Project.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _orderRepository;
        private readonly ICartRepo cartRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<APPUser> _userManager;

        public OrdersController(IOrderRepo orderRepository, ICartRepo cartRepository, IHttpContextAccessor _contextAccessor, UserManager<APPUser> manager)
        {
            _orderRepository = orderRepository;
            _contextAccessor = _contextAccessor;
            _userManager = manager;
            this.cartRepository = cartRepository;
        }
        [HttpGet]
        public IActionResult GetOrd()
        {
            var ord = _orderRepository.GetAll();
            return Ok(ord);
        }
        [HttpGet("order/{id:int}", Name = "GetOrderById")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = _userManager.GetUserId(User);
            var order = _orderRepository.GetById(id, userId);

            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            return Ok(order);
        }
        [HttpPost("AddOrder")]
        //[Authorize(Roles = "Admin")]

        public async Task<IActionResult> Add([FromBody] OrderDTO ord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }
             var cart =  cartRepository.GetById(user.Id);

            if (cart == null)
            {
                return NotFound("Cart not found.");
            }
            var item = cart.Items.FirstOrDefault(x => x.ProductId == ord.ProductId);

            if (item == null)
            {
                return BadRequest("Product not found in cart.");
            }
            var order = new Order
            {
                Address = ord.Address,
                OrderDate = DateTime.Now,
                Status = "Pending",
                UserId = user.Id,
                ProductId = ord.ProductId,
                Quantity = item.Quantity
            };
            order.CalculateTotalPrice(item.Product.Price);
             _orderRepository.AddOrder(order);
            var link = Url.Link("GetOrderById", new { id = order.Id });

            return Created(link, order);
        }
        [HttpPut("UpdateOrder/{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDTO orderDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
                if (user == null) 
                { 
                    return NotFound();
                }
                var order = _orderRepository.GetById(id, user.Id);
                if (order == null) 
                {
                    return NotFound("Order Not found");
                }
                if (order.UserId != user.Id)
                {
                    return Forbid();
                }
                _orderRepository.UpdateOrder(order);
                return Ok();
                
            }
            return BadRequest();
        }
        [HttpDelete("Delete")]
        public IActionResult DeleteById(int id)
        {
            var userId = _userManager.GetUserId(User);
            var existingOrder =  _orderRepository.GetById(id, userId);

            if (existingOrder == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            _orderRepository.DeleteOrder(id);
            return NoContent();
        }
    }
}
