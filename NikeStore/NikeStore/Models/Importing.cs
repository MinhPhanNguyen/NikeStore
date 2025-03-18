using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NikeStore.Models
{
    public class Importing
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImportID { get; set; }

        [Required(ErrorMessage = "Nhà cung cấp không được để trống.")]
        public int ProviderID { get; set; }
        public Provider Provider { get; set; }

        [Required(ErrorMessage = "Kho nhập hàng không được để trống.")]
        public int WarehouseID { get; set; }
        public Warehouse Warehouse { get; set; }

        public DateTime ImportDate { get; set; } = DateTime.Now;

        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn hoặc bằng 0.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Trạng thái nhập hàng không được để trống.")]
        [StringLength(20, ErrorMessage = "Trạng thái không được vượt quá 20 ký tự.")]
        public string Status { get; set; } = "Pending";

        public ICollection<ImportingDetail> ImportingDetails { get; set; }
    }

}
