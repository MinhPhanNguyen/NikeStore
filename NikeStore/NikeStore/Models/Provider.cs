using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NikeStore.Models
{
    public class Provider
    {
        [Key]
        public int ProviderID { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống.")]
        [StringLength(255, ErrorMessage = "Tên nhà cung cấp không được vượt quá 255 ký tự.")]
        public string ProviderName { get; set; }

        [StringLength(255, ErrorMessage = "Tên người liên hệ không được vượt quá 255 ký tự.")]
        public string ContactPerson { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự.")]
        public string Email { get; set; }

        public string Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
