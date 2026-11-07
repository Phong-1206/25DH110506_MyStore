using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace _25DH110506_MyStore.Models.Validation
{
    public class ImageFileAttribute : ValidationAttribute
    {
        // Cho phép cả .jpg và .jpeg
        private static readonly string[] AllowedExtensions =
            { ".jpg", ".jpeg", ".png", ".gif" };

        // Cho phép tất cả biến thể MIME mà các trình duyệt/OS gửi
        private static readonly string[] AllowedContentTypes =
        {
            "image/jpeg", "image/jpg", "image/pjpeg",   // JPEG variants
            "image/png",  "image/x-png",                // PNG variants
            "image/gif"                                 // GIF
        };

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var file = value as HttpPostedFileBase;

            // Không chọn file → OK (cho phép Edit giữ ảnh cũ)
            if (file == null || file.ContentLength == 0)
                return ValidationResult.Success;

            // Kiểm tra đuôi file — case-insensitive, có dấu chấm
            var ext = Path.GetExtension(file.FileName ?? "").ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                return new ValidationResult(ErrorMessage);

            // Kiểm tra ContentType — case-insensitive
            var ct = (file.ContentType ?? "").ToLowerInvariant();
            if (!AllowedContentTypes.Contains(ct))
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}