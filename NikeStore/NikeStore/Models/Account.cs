using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NikeStore.Repository.Validation;

namespace NikeStore.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [Required, StringLength(100, MinimumLength = 3, ErrorMessage = "Tên tài khoản phải từ 3 đến 100 ký tự.")]
        public string AccountName { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Mật khẩu phải từ 3 đến 100 ký tự.")]
        public string Password { get; set; }

        [Required, StringLength(100, MinimumLength = 3, ErrorMessage = "Email phải từ 3 đến 100 ký tự."),EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(15, MinimumLength = 3, ErrorMessage = "Số điện thoại phải từ 3 đến 15 ký tự."),Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string ImgUrl { get; set; } = "noimage.jpg";

        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
