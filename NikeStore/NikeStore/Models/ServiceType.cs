using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NikeStore.Models
{
    public class ServiceType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceTypeID { get; set; }

        [Required, StringLength(150, MinimumLength = 3, ErrorMessage = "Tên loại dịch vụ phải từ 3 đến 150 ký tự.")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        [Required(ErrorMessage = "Mô tả không được null")]
        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Link { get; set; }

        // Khóa ngoại tham chiếu đến Service
        public int ServiceID { get; set; }

        [ForeignKey("ServiceID")]
        public Service Service { get; set; }
    }
}
