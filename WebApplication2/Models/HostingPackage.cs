using System.ComponentModel.DataAnnotations;

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
    }
}
