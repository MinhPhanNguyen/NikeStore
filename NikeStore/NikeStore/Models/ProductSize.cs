using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NikeStore.Models
{
    public class ProductSize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductSizeID { get; set; }

        [Required, StringLength(100, MinimumLength = 3, ErrorMessage = "Tên danh mục phải từ 3 đến 100 ký tự.")]
        public string Size { get; set; }

        public ICollection<Product> Product { get; set; } = new List<Product>();
    }
}
