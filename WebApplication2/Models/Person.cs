using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [EmailAddress, Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign key
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
