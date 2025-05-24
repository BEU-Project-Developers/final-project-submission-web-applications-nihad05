namespace WebApplication2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}