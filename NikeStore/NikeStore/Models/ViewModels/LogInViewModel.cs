using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NikeStore.Repository.Validation;

namespace NikeStore.Models.ViewModels
{
    public class LogInViewModel
    {
        public int ID { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Mật khẩu phải từ 3 đến 100 ký tự.")]
        public string Password { get; set; }

        [Required, StringLength(100, MinimumLength = 3, ErrorMessage = "Email phải từ 3 đến 100 ký tự.")]
        public string AccountName { get; set; }

        public string ReturnUrl { get; set; }
    }
}
