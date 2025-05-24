namespace WebApplication2.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int OrderId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}