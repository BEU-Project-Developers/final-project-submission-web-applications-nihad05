namespace WebApplication2.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int? DomainId { get; set; }
        public int? HostingPlanId { get; set; }
        public decimal Price { get; set; }
    }
}