using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NikeStore.Models
{
    public class ProductColor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductColorID { get; set; }

        [Required, StringLength(500, ErrorMessage = "Mau sac không được vượt quá 500 ký tự.")]
        public string Color { get; set; }

        public ICollection<Product> Product { get; set; } = new List<Product>();
    }
}
