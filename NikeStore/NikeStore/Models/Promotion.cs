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
        [Required(ErrorMessage = "Name have no blank.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description have no blank.")]
        public string Description { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public decimal Discount { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        [Required( ErrorMessage = "Quantity must be between 0 and 500.")]
        public int Quantity { get; set; }

        public int Status { get; set; }
    }
}
