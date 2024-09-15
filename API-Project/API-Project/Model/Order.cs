namespace API_Project.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }= DateTime.Now;
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
        public string Address { get; set; }
        public bool IsInCart { get; set; } = true;
        public void MarkAsCompleted()
        {
            Status = "Completed";
            IsInCart = false; 
        }
        public void CalculateTotalPrice(decimal pricePerItem)
        {
            TotalPrice = Quantity * pricePerItem;
        }
        public bool IsPendingOrder()
        {
            return Status == "Pending" && IsInCart;
        }
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity > 0)
            {
                Quantity = newQuantity;
            }
        }

    }
}
