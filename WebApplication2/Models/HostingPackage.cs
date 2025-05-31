using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class HostingPackage
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int StorageGB { get; set; }

        public int BandwidthGB { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        public string? Description { get; set; }
        // Foreign key
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
