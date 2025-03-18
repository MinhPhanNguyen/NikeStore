using System.ComponentModel.DataAnnotations.Schema;

namespace NikeStore.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public long ProductId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
