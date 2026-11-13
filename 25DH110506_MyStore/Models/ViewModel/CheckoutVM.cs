using System.ComponentModel.DataAnnotations;

namespace _25DH110506_MyStore.Models.ViewModel
{
    public class CheckoutVM
    {
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string AddressDelivery { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; } // "COD" (Tiền mặt), "Bank" (Chuyển khoản)

        public CartVM Cart { get; set; } // Chứa danh sách sản phẩm để hiển thị lại
    }
}