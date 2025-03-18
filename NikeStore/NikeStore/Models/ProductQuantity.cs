using System.ComponentModel.DataAnnotations;

namespace NikeStore.Models
{
    public class ProductQuantity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Số lượng không được để trống.")]
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public long ProductId { get; set; }
    }
}
