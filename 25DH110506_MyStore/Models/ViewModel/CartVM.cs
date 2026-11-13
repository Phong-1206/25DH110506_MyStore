using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _25DH110506_MyStore.Models.ViewModel
{
    /// <summary>Một dòng trong giỏ hàng</summary>
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }

        public decimal SubTotal => ProductPrice * Quantity;
    }

    /// <summary>Toàn bộ giỏ hàng</summary>
    public class CartVM
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public int TotalItems => Items?.Sum(i => i.Quantity) ?? 0;
        public decimal TotalPrice => Items?.Sum(i => i.SubTotal) ?? 0m;
        public bool IsEmpty => Items == null || Items.Count == 0;
    }
}