using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Domain
    {
        public int Id { get; set; }

        [Required]
        public string DomainName { get; set; }

        public int Price { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Foreign key
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
