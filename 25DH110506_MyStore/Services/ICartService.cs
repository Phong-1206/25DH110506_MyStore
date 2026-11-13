using _25DH110506_MyStore.Models.ViewModel;

namespace _25DH110506_MyStore.Services
{
    public interface ICartService
    {
        CartVM GetCart();
        void AddItem(int productId, int quantity);
        void RemoveItem(int productId);
        void ClearCart();
        void UpdateQuantity(int productId, int quantity);
        int GetTotalItems();
    }
}