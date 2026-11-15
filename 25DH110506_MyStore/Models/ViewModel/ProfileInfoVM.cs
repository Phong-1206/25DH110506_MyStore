using System.ComponentModel.DataAnnotations;

namespace _25DH110506_MyStore.Models.ViewModel
{
    public class ProfileInfoVM
    {
        public string Username { get; set; } // Chỉ hiển thị, không cho sửa

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100, ErrorMessage = "Họ tên không quá 100 ký tự")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [StringLength(500, ErrorMessage = "Địa chỉ không quá 500 ký tự")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string Address { get; set; }
    }
}