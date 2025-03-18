using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NikeStore.Models
{
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }  // Khóa chính

        [Required]
        [StringLength(50)]
        public string Name { get; set; }  // Ví dụ: "Momo", "Visa", "PayPal"

        [StringLength(255)]
        public string Description { get; set; }  // Mô tả thêm nếu cần

        // Liên kết với Payment
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
