using API_Project.DTO;
using API_Project.IRepo;
using API_Project.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IProductrepo _productRepository;
        private readonly UserManager<APPUser> userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICartRepo _cartRepository;


        public CartController(IProductrepo productRepository, UserManager<APPUser> userManager, IHttpContextAccessor httpContextAccessor, ICartRepo cartRepository)
        {
            _productRepository = productRepository;
            this.userManager = userManager;
            _contextAccessor = httpContextAccessor;
            _cartRepository = cartRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
           var user =  userManager.GetUserId(User);
            var cart = _cartRepository.GetById(user);
            return Ok(cart);
        }
        [HttpPost("addtocart")]
        public async Task<IActionResult> AddCart(CartItemDTO cartItemDTO)
        {
            var product = _productRepository.GetById(cartItemDTO.ProductId);
            if (product == null)
            {
                return NotFound("There is No product to add");
            }
            var user = await userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            var cart = _cartRepository.GetById(user.Id);
            if (cart == null) 
            { 
                cart=new Cart()
                {
                    UserId = user.Id,
                };
                _cartRepository.AddToCart(cart);
            }
            var item= cart.Items.FirstOrDefault(x=>x.Id==product.Id);
            if (item == null)
            {
                cart.Items.Add(new CartItem(){
                    Product=product,
                    Quantity=cartItemDTO.Quantity,
                    ProductId=product.Id,
                });
            }
            else
            {
                item.Quantity += cartItemDTO.Quantity;
            }
            _cartRepository.UpdateCart(cart);
            return Ok(new
            {
                Message = "Item Added To Cart",
                TotalPrice = cart.TotalPrice
            });
        }
        [HttpDelete("remove")]   
        public IActionResult RemoveItemFromCart(int Productid)
        {
            var user = userManager.GetUserId(User);
            if (user == null)
            {
                return Unauthorized();
            }
            _cartRepository.DeleteFromCart(user, Productid);
            return Ok("Item Removed");
        }

    }
}
