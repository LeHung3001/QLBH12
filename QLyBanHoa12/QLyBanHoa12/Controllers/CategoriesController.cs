using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLyBanHoa12.Models;

namespace QLyBanHoa12.Controllers
{
    public class CategoriesController : Controller
    {
        DbBanHoa12Context da = new DbBanHoa12Context();
        [HttpGet("Get all categories")]
        public IActionResult GetAllCategories()
        {
            var ds = da.Categories.ToList();
            return Ok(ds);
        }

        [HttpGet(" Get category by ID")]
        public IActionResult GetCategoryByID(int id)
        {
            var ds = da.Categories.FirstOrDefault(s => s.CategoryId == id);
            return Ok(ds);
        }


        [HttpPost("Add a new Category")]
        public void AddCategory([FromBody] LoaiSanPham loaiSanPham)
        {
            Category p = new Category();
            p.CategoryName = loaiSanPham.CategoryName;
            p.Description = loaiSanPham.Description;
            da.Categories.Add(p);
            da.SaveChanges();
        }

        [HttpPut("Edit a category")]
        public void EditCategory([FromBody] LoaiSanPham loaiSanPham)
        {
            Category p = da.Categories.FirstOrDefault(s => s.CategoryId == loaiSanPham.CategoryId);
            p.CategoryName = loaiSanPham.CategoryName;
            p.Description= loaiSanPham.Description;
            da.Categories.Update(p);
            da.SaveChanges();
        }

        [HttpDelete("Delete a Category")]
        public void DeleteCategory(int id)
        {
            Category p = da.Categories.FirstOrDefault(s => s.CategoryId== id);
            da.Categories.Remove(p);
            da.SaveChanges();
        }

        [HttpPost("Search category by name")]
        public IActionResult SearchCategory([FromBody] SearchCategory searchCategory)
        {
            var categories = da.Categories.Where(s => s.CategoryName.Contains(searchCategory.Keyword)).ToList();
            var offset = (searchCategory.Page - 1) * searchCategory.Size;
            int total = categories.Count;
            int totalPage = (total % searchCategory.Size) == 0 ? (int)(total / searchCategory.Size) : (int)(1 + (total / searchCategory.Size));
            var data = categories.OrderBy(x => x.CategoryName).Skip(offset).Take(searchCategory.Size).ToList();
            /*var res = new
            {
                Data = data,
                TotalRecord = total,
                TotalPage = totalPage,
                Page = searchProduct.Page
            };*/
            return Ok(categories);
        }
    }
}
