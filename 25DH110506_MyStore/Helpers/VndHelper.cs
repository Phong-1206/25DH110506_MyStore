using System.Web.Mvc;

namespace _25DH110506_MyStore.Helpers
{
    public static class VndHelper
    {
        /// <summary>
        /// @Html.Vnd(27990000) → "27,990,000"
        /// </summary>
        public static string Vnd(this HtmlHelper html, decimal value)
        {
            return value.ToVnd();
        }

        public static string Vnd(this HtmlHelper html, decimal? value)
        {
            return value.HasValue ? value.Value.ToVnd() : "0";
        }

        public static string Vnd(this HtmlHelper html, int value)
        {
            return value.ToVnd();
        }

        public static string Vnd(this HtmlHelper html, int? value)
        {
            return value.HasValue ? value.Value.ToVnd() : "0";
        }

        /// <summary>
        /// @Html.VndWithSymbol(27990000) → "27,990,000 ₫"
        /// </summary>
        public static string VndWithSymbol(this HtmlHelper html, decimal value)
        {
            return value.ToVndWithSymbol();
        }

        public static string VndWithSymbol(this HtmlHelper html, decimal? value)
        {
            return value.HasValue ? value.Value.ToVndWithSymbol() : "0 ₫";
        }
    }
}