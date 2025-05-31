using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } // Nullable to track updates

        public DateTime? DeletedAt { get; set; } // Nullable for soft deletes

        // Navigation
        public List<Person> Persons { get; set; } = new();
        public List<Domain> Domains { get; set; } = new();
        public List<CompanyHosting> CompanyHostings { get; set; } = new();
    }
}
