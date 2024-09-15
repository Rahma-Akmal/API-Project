namespace API_Project.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }=DateTime.Now;
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }

    }
}
