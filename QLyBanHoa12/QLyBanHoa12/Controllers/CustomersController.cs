using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLyBanHoa12.Models;

namespace QLyBanHoa12.Controllers
{
    public class CustomersController : Controller
    {
        DbBanHoa12Context da = new DbBanHoa12Context();
        [HttpGet("Get all Customer")]
        public IActionResult GetAllCustomer()
        {
            var ds = da.Customers.ToList();
            return Ok(ds);
        }
        [HttpGet("Get Customer by Id")]
        public IActionResult GetCustomerById(string id)
        {
            var customer = da.Customers.FirstOrDefault(s => s.CustomerId == id);
            return Ok(customer);
        }


        [HttpPost("Add new customer")]
        public void AddCustomer([FromBody] KhachHang khachHang)
        {
            Customer p = new Customer();
            p.CustomerName = khachHang.CustomerName;
            p.CustomerId = khachHang.CustomerId;
            p.Phone = khachHang.Phone;
            p.Address = khachHang.Address;
            da.Customers.Add(p);
            da.SaveChanges();
        }



        [HttpPut("Edit Customer")]
        public void EditCustomer([FromBody] KhachHang khachHang)
        {
            Customer p = da.Customers.FirstOrDefault(s => s.CustomerId == khachHang.CustomerId);
            p.CustomerName = khachHang.CustomerName;
            p.CustomerId = khachHang.CustomerId;
            p.Phone = khachHang.Phone;
            p.Address = khachHang.Address;
            da.Customers.Update(p);
            da.SaveChanges();
        }


        [HttpDelete("Delete customer")]
        public void DeleteCustomer(string id)
        {
            Customer p = da.Customers.FirstOrDefault(s => s.CustomerId == id);
            da.Customers.Remove(p);
            da.SaveChanges();
        }
        [HttpPost("Search customer by name")]
        public IActionResult SearchCustomer([FromBody] SearchCustomerReq searchCustomerReq)
        {
            var customers = da.Customers.Where(s => s.CustomerName.Contains(searchCustomerReq.Keyword)).ToList();
            //phân trang
            //1. trang 1 có 10 khách hàng : size
            //2. tổng khách hàng là 20 =>> 2 trang, 21 khách hàng =>> 3 trang.
            // trang số 2 là khách hàng tù 11 -> 20.

            var offset = (searchCustomerReq.Page - 1) * searchCustomerReq.Size;
            var total = customers.Count();
            int totalPage = (total % searchCustomerReq.Size) == 0 ? (int)(total / searchCustomerReq.Size) :
                (int)(1 + (total / searchCustomerReq.Size));
            var data = customers.OrderBy(x => x.CustomerId).Skip(offset).Take(searchCustomerReq.Size).ToList();

            return Ok(customers);
        }

    }





}

