using System.ComponentModel.DataAnnotations;

namespace NikeStore.Models
{
    public class Warehouse
    {
        [Key]
        public int WarehouseID { get; set; }

        [Required(ErrorMessage = "Tên kho hàng không được để trống.")]
        [StringLength(255, ErrorMessage = "Tên kho hàng không được vượt quá 255 ký tự.")]
        public string WarehouseName { get; set; }

        [Required(ErrorMessage = "Vị trí kho không được để trống.")]
        public string Location { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Sức chứa phải lớn hơn hoặc bằng 0.")]
        public int Capacity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Product> Product { get; set; }
        public ICollection<Importing> Importing { get; set; }
    }

}
