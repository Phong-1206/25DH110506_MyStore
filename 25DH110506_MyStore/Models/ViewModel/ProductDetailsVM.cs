using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace _25DH110506_MyStore.Models.ViewModel
{
    public class ProductDetailsVM
    {
        public Product product { get; set; }

        public int quantity { get; set; } = 1;
        public decimal estimatedValue { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 8;

        public List<Product> RelatedProducts { get; set; }

        public PagedList.IPagedList<Product> TopProducts { get; set; }
    }
}