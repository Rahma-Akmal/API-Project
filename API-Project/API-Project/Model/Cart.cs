namespace API_Project.Model
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(item => item.Product.Price * item.Quantity);
            }
        }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }

}
