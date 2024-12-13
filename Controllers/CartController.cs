using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


    public class CartController(DataContext db) : Controller
    {
        // this controller depends on the DataContext
        private readonly DataContext _dataContext = db;
        //public IActionResult Index() => View(_dataContext.CartItems.Include("Customer").Where(c => c.Customer.Email == User.Identity.Name));

        public IActionResult Index() => View(new DiscountCartModel { 
            CartItems = _dataContext.CartItems.Include("Customer").Where(c => c.Customer.Email == User.Identity.Name)
            , Discounts = _dataContext.Discounts.Include("Product").Where(d => d.Product.ProductId == d.ProductId)
            , Customers = _dataContext.Customers.Where(c => c.Email == User.Identity.Name)
            , Orders = _dataContext.Orders.Include("Customer").Where(o => o.Customer.Email == User.Identity.Name)
            //, OrderDetails = _dataContext.OrderDetails.Where(o => o.Product.ProductId == o.ProductId)
            });
    }
