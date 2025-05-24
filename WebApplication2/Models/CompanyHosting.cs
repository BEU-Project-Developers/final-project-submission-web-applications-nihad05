using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class CompanyHosting
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Foreign keys
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int HostingPackageId { get; set; }
        public HostingPackage HostingPackage { get; set; }
    }
}
