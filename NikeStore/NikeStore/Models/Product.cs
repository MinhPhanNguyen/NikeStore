using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NikeStore.Repository.Validation;

namespace NikeStore.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductID { get; set; }

        [Required, StringLength(150, MinimumLength = 3, ErrorMessage = "Tên sản phẩm phải từ 3 đến 150 ký tự.")]
        public string Name { get; set; }

        public string Slug { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        [Required(ErrorMessage = "Mô tả không được để trống.")]
        public string Description { get; set; }

        public int Quantity { get; set; }
        public int Sold { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 100000000, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Danh mục là bắt buộc.")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Màu sắc là bắt buộc.")]
        public int ProductColorID { get; set; }

        [Required(ErrorMessage = "Kích thước là bắt buộc.")]
        public int ProductSizeID { get; set; }

        [Required(ErrorMessage = "Loại là bắt buộc.")]
        public int ProductTypeID { get; set; }

        [Required(ErrorMessage = "Giới tính là bắt buộc.")]
        public int GenderID { get; set; }

        [Required(ErrorMessage = "Kho là bắt buộc.")]
        public int WarehouseID { get; set; }
        [Required(ErrorMessage = "Mã khuyến mãi là bắt buộc.")]
        public int PromotionID { get; set; }

        public bool IsHot { get; set; } = false;

        public bool IsFavorite { get; set; } = false;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("CategoryID")]
        public ProductCategory ProductCategory { get; set; }
        [ForeignKey("ProductColorID")]
        public ProductColor ProductColor { get; set; }
        [ForeignKey("ProductSizeID")]
        public ProductSize ProductSize { get; set; }
        [ForeignKey("ProductTypeID")]
        public ProductType ProductType { get; set; }
        [ForeignKey("GenderID")]
        public ProductGender ProductGender { get; set; }
        [ForeignKey("WarehouseID")]
        public Warehouse Warehouse { get; set; }
        [ForeignKey("PromotionID")]
        public Promotion Promotion { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ImportingDetail> ImportingDetails { get; set; }

        public string? ImageUrl { get; set; } = "noimage.jpg";
        [NotMapped]
        [FileExtension]
        public List<IFormFile>? ImageUploads { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
