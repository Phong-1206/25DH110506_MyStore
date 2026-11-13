using System;
using System.Globalization;

namespace _25DH110506_MyStore.Helpers
{
    public static class MoneyExtensions
    {
        /// Format số tiền thành chuỗi có dấu phẩy phân cách: 27990000 → "27,990,000"
        public static string ToVnd(this decimal value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }

        public static string ToVnd(this decimal? value)
        {
            return value.HasValue ? value.Value.ToVnd() : "0";
        }

        public static string ToVnd(this int value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }

        public static string ToVnd(this int? value)
        {
            return value.HasValue ? value.Value.ToVnd() : "0";
        }

        /// Format kèm ký hiệu: 27990000 → "27,990,000 ₫"
        public static string ToVndWithSymbol(this decimal value)
        {
            return value.ToVnd() + " ₫";
        }

        public static string ToVndWithSymbol(this decimal? value)
        {
            return value.HasValue ? value.Value.ToVndWithSymbol() : "0 ₫";
        }
    }
}