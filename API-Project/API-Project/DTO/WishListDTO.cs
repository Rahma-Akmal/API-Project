using API_Project.Model;
using System.Text.Json.Serialization;

namespace API_Project.DTO
{
    public class WishListDTO
    {
        public string UserId { get; set; }
        public int WishlistId { get; set; }
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}
