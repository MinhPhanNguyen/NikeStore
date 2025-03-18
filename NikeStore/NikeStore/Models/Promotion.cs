using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NikeStore.Models
{
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public double? Discount { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }  

        public bool IsActive { get; set; } = true;
        public ICollection<Product> Product { get; set; } = new List<Product>();
    }
}
