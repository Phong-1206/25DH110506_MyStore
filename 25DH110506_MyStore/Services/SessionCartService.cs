using System.Collections.Generic;
using System.Linq;
using System.Web;
using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Models.ViewModel;

namespace _25DH110506_MyStore.Services
{
    public class SessionCartService : ICartService
    {
        private const string CartSessionKey = "Cart";
        private readonly MyStoreEntities db;
        private readonly HttpContextBase httpContext;

        public SessionCartService(MyStoreEntities db, HttpContextBase httpContext)
        {
            this.db = db;
            this.httpContext = httpContext;
        }

        private List<CartItem> GetCartItems()
        {
            var items = httpContext.Session[CartSessionKey] as List<CartItem>;
            if (items == null)
            {
                items = new List<CartItem>();
                httpContext.Session[CartSessionKey] = items;
            }
            return items;
        }

        private void SaveCartItems(List<CartItem> items)
        {
            httpContext.Session[CartSessionKey] = items;
        }

        public CartVM GetCart()
        {
            return new CartVM { Items = GetCartItems() };
        }

        public void AddItem(int productId, int quantity)
        {
            if (quantity < 1) quantity = 1;

            var items = GetCartItems();
            var existing = items.FirstOrDefault(i => i.ProductID == productId);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                var product = db.Products.Find(productId);
                if (product == null) return;

                items.Add(new CartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    ProductPrice = product.ProductPrice,
                    Quantity = quantity
                });
            }

            SaveCartItems(items);
        }

        public void RemoveItem(int productId)
        {
            var items = GetCartItems();
            items.RemoveAll(i => i.ProductID == productId);
            SaveCartItems(items);
        }

        public void ClearCart()
        {
            httpContext.Session[CartSessionKey] = new List<CartItem>();
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            if (quantity < 1) quantity = 1;
            if (quantity > 99) quantity = 99;

            var items = GetCartItems();
            var existing = items.FirstOrDefault(i => i.ProductID == productId);
            if (existing == null) return;

            existing.Quantity = quantity;
            SaveCartItems(items);
        }

        public int GetTotalItems()
        {
            return GetCartItems().Sum(i => i.Quantity);
        }
    }
}