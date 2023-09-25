using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLyBanHoa12.Models;

namespace QLyBanHoa12.Controllers
{
    public class StatisticsController : Controller
    {
        DbBanHoa12Context da = new DbBanHoa12Context();
        
        // Tong thanh tien theo nam
        [HttpPost("Cal total by year")]
        public IActionResult CalToTalByYear(int year)
        {
            var ds = da.Orders.Where(s => s.OrderDate.Value.Year == year)
              .Join(da.OrderDetails, o => o.OrderId, d => d.OrderId, (o, d) =>
                new
                {
                    nam = o.OrderDate.Value.Year,
                    total = d.Quantity * d.UnitPrice
                })
               .GroupBy(s => s.nam).Select(s => new { s.Key, total = s.Sum(g => g.total) });
            return Ok(ds);
        }

        // Tong so luong don hang cua moi khach hang
        [HttpPost("Cal orders by customer")]
        public IActionResult CalToTalByCus()
        {
            var ds = da.Orders.GroupBy(s => s.Customer.CustomerName).Select(s => new { s.Key, sl = s.Count() });
            var totalOrder = ds.ToList().Count();
            var total = ds.ToList().Sum(s => s.sl);

            var res = new
            {
                Data = ds,
                TotalOrder = totalOrder,
                Total = total
            };

            return Ok(res);
        }
    }
}
