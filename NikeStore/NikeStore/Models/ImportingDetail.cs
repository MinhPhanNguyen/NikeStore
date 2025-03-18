using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NikeStore.Models
{
    public class ImportingDetail
    {
        [Key]
        public int ImportDetailID { get; set; }

        [Required(ErrorMessage = "Mã phiếu nhập không được để trống.")]
        public int ImportID { get; set; }
        public Importing Importing { get; set; }

        [Required(ErrorMessage = "Mã sản phẩm không được để trống.")]
        public long ProductID { get; set; }
        public Product Product { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
