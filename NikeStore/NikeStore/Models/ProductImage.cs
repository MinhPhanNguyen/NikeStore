using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NikeStore.Models
{
    public class ProductImage
    {
        [Key]
        public int ProductImageID { get; set; }
        public string ImageUrl { get; set; }

        public long ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
