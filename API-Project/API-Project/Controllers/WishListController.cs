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
    public class WishListController : ControllerBase
    {
        private readonly IWishListRepo _wishListRepo;
        private readonly UserManager<APPUser> _userManager;

        public WishListController(IWishListRepo wishListRepo, UserManager<APPUser> userManager)
        {
            _wishListRepo = wishListRepo;
            _userManager = userManager;
        }
        [HttpGet]
        [HttpGet]
        [Authorize]
        public IActionResult GetUserWishList()
        {
            var userId = _userManager.GetUserId(User); 
            var wishList = _wishListRepo.GetByUserId(userId);
            if (wishList == null)
            {
                return NotFound("Wishlist not found for user.");
            }
            return Ok(wishList);
        }
        [HttpPost("add")]
        //[Authorize]
        public IActionResult AddToWishlist([FromBody] int productId)
        {
            var userId = _userManager.GetUserId(User);
            var wishlist = _wishListRepo.GetByUserId(userId);

            if (wishlist == null)
            {
                wishlist = new WishList
                {
                    UserId = userId,
                };
                _wishListRepo.Add(wishlist);
            }

            if (wishlist.WishListItem.Any(w => w.ProductId == productId))
            {
                return BadRequest("Product is already in the wishlist.");
            }

            var newItem = new WishListItem
            {
                ProductId = productId,
                Wishlist = wishlist
            };

            wishlist.WishListItem.Add(newItem);
            _wishListRepo.Update(wishlist);

            return Ok(wishlist);
        }

        [HttpDelete("remove/{productId}")]
        [Authorize]
        public IActionResult RemoveFromWishlist(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var wishlist = _wishListRepo.GetByUserId(userId);

            if (wishlist == null)
            {
                return NotFound("Wishlist not found.");
            }

            var itemToRemove = wishlist.WishListItem.FirstOrDefault(i => i.ProductId == productId);
            if (itemToRemove == null)
            {
                return NotFound("Product not found in the wishlist.");
            }

            wishlist.WishListItem.Remove(itemToRemove);
            _wishListRepo.Update(wishlist);

            return Ok(wishlist);
        }
    }
}
