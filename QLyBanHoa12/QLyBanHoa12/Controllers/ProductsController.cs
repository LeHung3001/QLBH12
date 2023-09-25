using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLyBanHoa12.Models;

namespace QLyBanHoa12.Controllers
{
    public class ProductsController : Controller
    {
        DbBanHoa12Context da = new DbBanHoa12Context();
        
        // Lay ds toan bo san pham
        [HttpGet("Get all products")]
        public IActionResult GetAllProducts()
        {
            var ds = da.Products.ToList();
            return Ok(ds);

        }
        // Tim san pham theo ID
        [HttpGet(" Get product by ID")]
        public IActionResult GetProductByID(int id)
        {
            var ds = da.Products.FirstOrDefault(s => s.ProductId == id);
            return Ok(ds);
        }

        // Them san pham
        [HttpPost("Add a new product")]
        public void AddProduct([FromBody] SanPham sanPham)
        {
            using (var tran = da.Database.BeginTransaction())
            {
                try
                {
                    Product p = new Product();
                    p.ProductName = sanPham.ProductName;
                    p.SupplierId = sanPham.SupplierId;
                    p.CategoryId = sanPham.CategoryId;
                    p.UnitPrice = sanPham.UnitPrice;
                    da.Products.Add(p);
                    da.SaveChanges();
                }
                catch (Exception)
                {
                    tran.Rollback();
                } 
            }
        }

        // Chinh sua thong tin san pham
        [HttpPut("Edit a product")]
        public void EditProduct([FromBody] SanPham sanPham)
        {
            using (var tran = da.Database.BeginTransaction())
            {
                try
                {
                    Product p = da.Products.FirstOrDefault(s => s.ProductId == sanPham.ProductId);
                    p.ProductName = sanPham.ProductName;
                    p.SupplierId = sanPham.SupplierId;
                    p.CategoryId = sanPham.CategoryId;
                    p.UnitPrice = sanPham.UnitPrice;
                    da.Products.Update(p);
                    da.SaveChanges();
                }
                catch (Exception)
                {
                    tran.Rollback();
                } 
            }
        }

        //
        [HttpDelete("Delete a product")]
        public void DeleteProduct(int id)
        {
            Product p = da.Products.FirstOrDefault(s => s.ProductId == id);
            da.Products.Remove(p);
            da.SaveChanges();
        }

        [HttpPost("Search product by name")]
        public IActionResult SearchProduct([FromBody] SearchProduct searchProduct)
        {
            var products = da.Products.Where(s => s.ProductName.Contains(searchProduct.Keyword)).ToList();
            var offset = (searchProduct.Page - 1) * searchProduct.Size;
            int total = products.Count;
            int totalPage = (total % searchProduct.Size) == 0 ? (int)(total / searchProduct.Size) : (int)(1 + (total / searchProduct.Size));
            var data = products.OrderBy(x=>x.ProductId).Skip(offset).Take(searchProduct.Size).ToList();
           
            return Ok(products);
        }

    }
}
