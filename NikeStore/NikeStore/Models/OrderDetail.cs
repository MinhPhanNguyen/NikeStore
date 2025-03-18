using System.ComponentModel.DataAnnotations.Schema;

namespace NikeStore.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OrderCode { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
