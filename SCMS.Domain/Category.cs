using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCMS.Domain
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        // Navigation property: Một Category có nhiều MenuItem
        public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}