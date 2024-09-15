using System.Text.Json.Serialization;

namespace API_Project.Model
{
    public class WishList
    {
        public int Id { get; set; } 
        public string? UserId { get; set; }
        [JsonIgnore]
        public virtual List<WishListItem> WishListItem { get; set; }=new List<WishListItem>();
    }
}
