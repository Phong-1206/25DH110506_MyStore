using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;

using _25DH110506_MyStore.Models;
using _25DH110506_MyStore.Models.ViewModel;
namespace _25DH110506_MyStore.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Products
        public ActionResult Index(string SearchTerm, decimal? minPrice, decimal? maxPrice, string sortOrder, int? page)
        {
            var model = new ProductSearchVM();
            var products = db.Products.AsQueryable();
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(SearchTerm) ||
                    p.ProductDescription.Contains(SearchTerm) ||
                    p.Category.CategoryName.Contains(SearchTerm));
            }
            var result = products.ToList();
            System.Diagnostics.Debug.WriteLine($"=== FINAL COUNT:      {result.Count}");
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.ProductPrice <= maxPrice.Value);
            }
            switch (sortOrder) 
            {
                case "name_asc":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.ProductPrice);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }
            model.SortOrder = sortOrder;
            int pageNumber = page ?? 1;
            int pageSize = 2;
            model.Products = products.ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            /////ModelState.Remove("UploadImg");
            if (ModelState.IsValid)
            {
                if (product.UploadImg != null && product.UploadImg.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(product.UploadImg.FileName);
                    string extension = Path.GetExtension(product.UploadImg.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

                    string folderPath = Server.MapPath("~/Content/images/");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string savePath = Path.Combine(folderPath, fileName);
                    product.UploadImg.SaveAs(savePath);
                    product.ProductImage = "~/Content/images/" + fileName;
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
     
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                // 1. Lấy bản ghi cũ (AsNoTracking để không conflict với entity mới)
                var existing = db.Products.AsNoTracking()
                                   .FirstOrDefault(p => p.ProductID == product.ProductID);
                if (existing == null) return HttpNotFound();

                // 2. Xử lý ảnh
                if (product.UploadImg != null && product.UploadImg.ContentLength > 0)
                {
                    string folderPath = Server.MapPath("~/Content/images/");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    // 2a. XÓA ẢNH CŨ nếu file còn tồn tại trên ổ cứng
                    if (!string.IsNullOrEmpty(existing.ProductImage))
                    {
                        string oldPhysicalPath = Server.MapPath(existing.ProductImage);
                        if (System.IO.File.Exists(oldPhysicalPath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldPhysicalPath);
                                System.Diagnostics.Debug.WriteLine($"=== Đã xóa ảnh cũ: {oldPhysicalPath}");
                            }
                            catch (Exception ex)
                            {
                                // Không throw — chỉ log, vẫn tiếp tục lưu ảnh mới
                                System.Diagnostics.Debug.WriteLine($"=== Không xóa được ảnh cũ: {ex.Message}");
                            }
                        }
                        else
                        {
                            // File cũ không tồn tại → không cần xóa, chỉ cập nhật DB
                            System.Diagnostics.Debug.WriteLine($"=== Ảnh cũ không tồn tại: {oldPhysicalPath}");
                        }
                    }

                    // 2b. ĐỔI TÊN ẢNH MỚI (tránh trùng bằng timestamp)
                    string baseName = Path.GetFileNameWithoutExtension(product.UploadImg.FileName);
                    string extension = Path.GetExtension(product.UploadImg.FileName);
                    string newFileName = baseName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

                    // 2c. Safety: nếu trùng tên (gần như không bao giờ) → thêm random
                    string savePath = Path.Combine(folderPath, newFileName);
                    if (System.IO.File.Exists(savePath))
                    {
                        newFileName = baseName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff")
                                    + "_" + Guid.NewGuid().ToString("N").Substring(0, 6) + extension;
                        savePath = Path.Combine(folderPath, newFileName);
                    }

                    // 2d. Lưu file mới
                    product.UploadImg.SaveAs(savePath);

                    // 2e. Cập nhật DB với đường dẫn mới
                    product.ProductImage = "~/Content/images/" + newFileName;

                    System.Diagnostics.Debug.WriteLine($"=== Đã lưu ảnh mới: {savePath}");
                }
                else
                {
                    // 3. Không chọn ảnh mới → GIỮ NGUYÊN ảnh cũ trong DB
                    product.ProductImage = existing.ProductImage;
                }

                // 4. Cập nhật entity
                db.Entry(product).State = EntityState.Modified;
                db.Entry(product).Property(p => p.ProductImage).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // ModelState fail → load lại dropdown + trả về view
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            if (!string.IsNullOrEmpty(product.ProductImage))
            {
                string physicalPath = Server.MapPath(product.ProductImage);
                if (System.IO.File.Exists(physicalPath))
                    System.IO.File.Delete(physicalPath);
            }
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
