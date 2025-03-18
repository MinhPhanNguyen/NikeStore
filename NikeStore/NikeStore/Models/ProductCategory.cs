using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NikeStore.Repository.Validation;

namespace NikeStore.Models
{
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [Required, StringLength(100, MinimumLength = 3, ErrorMessage = "Tên danh mục phải từ 3 đến 100 ký tự.")]
        public required string CategoryName { get; set; }
        public required string Slug { get; set; }

        [Required,StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string Description { get; set; }
        public string? ImageUrl { get; set; } = "noimage.jpg";

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUploads { get; set; }

        // Thiết lập quan hệ với bảng Product
        public ICollection<Product> Product { get; set; } = new List<Product>();
    }
}
