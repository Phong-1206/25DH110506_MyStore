using System.Collections.Generic;

namespace _25DH110506_MyStore.Models.ViewModel
{
    public class DashboardVM
    {
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalRevenue { get; set; }

        // Dữ liệu cho biểu đồ tròn
        public List<string> OrderStatusLabels { get; set; }
        public List<int> OrderStatusData { get; set; }
    }
}