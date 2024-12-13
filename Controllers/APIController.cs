using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Northwind.Controllers
{
    public class APIController(DataContext db) : Controller
    {
        // this controller depends on the NorthwindRepository
        private readonly DataContext _dataContext = db;

        [HttpGet, Route("api/product")]
        // returns all products
        public IEnumerable<Product> Get() => _dataContext.Products.OrderBy(p => p.ProductName);

        [HttpGet, Route("api/product/{id}")]
        // returns specific product
        public Product Get(int id) => _dataContext.Products.FirstOrDefault(p => p.ProductId == id);

        [HttpGet, Route("api/product/discontinued/{discontinued}")]
        // returns all products where discontinued = true/false
        public IEnumerable<Product> GetDiscontinued(bool discontinued) => _dataContext.Products.Where(p => p.Discontinued == discontinued).OrderBy(p => p.ProductName);

        [HttpGet, Route("api/category/{CategoryId}/product")]
        // returns all products in a specific category
        public IEnumerable<Product> GetByCategory(int CategoryId) => _dataContext.Products.Where(p => p.CategoryId == CategoryId).OrderBy(p => p.ProductName);

        [HttpGet, Route("api/category/{CategoryId}/product/discontinued/{discontinued}")]
        // returns all products in a specific category where discontinued = true/false
        public IEnumerable<Product> GetByCategoryDiscontinued(int CategoryId, bool discontinued) => _dataContext.Products.Where(p => p.CategoryId == CategoryId && p.Discontinued == discontinued).OrderBy(p => p.ProductName);

        [HttpGet, Route("api/category")]
        // returns all categories
        //public IEnumerable<Category> GetCategory() => _dataContext.Categories.OrderBy(c => c.CategoryName);
        public IEnumerable<Category> GetCategory() => _dataContext.Categories.Include("Products").OrderBy(c => c.CategoryName);

        [HttpPost, Route("api/addtocart")]
        // adds a row to the cartitem table
        public CartItem Post([FromBody] CartItemJSON cartItem) => _dataContext.AddToCart(cartItem);

//** FINAL PROJECT **
        /*[HttpGet, Route("api/cart")]
        // gets all rows from  cartitem table
        public IEnumerable<CartItem> GetCartItems() => _dataContext.CartItems.OrderBy(c => c.CartItemId);
*/
        [HttpGet, Route("api/cart/product")]
        // gets all products from cartitem table
        public IEnumerable<CartItem> GetCartProduct() => _dataContext.CartItems.Include("Product").OrderBy(c => c.CustomerId);

        [HttpGet, Route("api/cart/customer/{id}")]
        // gets all products from cartitem table
        public IEnumerable<CartItem> GetCartProductCustomerId(int id) => _dataContext.CartItems.Include("Product").Include("Customer").Where(p => p.CustomerId == id).OrderByDescending(c => c.CartItemId);

        [HttpDelete, Route("api/cart/delete/{id}")]
        // deletes a row from the cartitem table
        public CartItem DeleteCartItem(int id) => _dataContext.DeleteCartItems(id);

        /*[HttpGet, Route("api/cart/discounted")]
        // gets all discounted products from cartitem table
        public IEnumerable<Discount> GetCartProductsDiscounted(int id) => _dataContext.Discounts.Include("Product").OrderBy(p => p.ProductId );*/

        [HttpGet, Route("api/cart/discounted/{id}")]
        // gets one discounted product from cartitem table
        public IEnumerable<Discount> GetCartProductDiscounted(int id) => _dataContext.Discounts.Include("Product").Where(p => p.ProductId == id);

        //get customer
        [HttpGet, Route("api/customer/{id}")]
        // gets all customers from customer table
        public IEnumerable<Customer> GetCustomers(int id) => _dataContext.Customers.Where(c => c.CustomerId == id);

        [HttpPost, Route("api/addtoorder")]
        // adds a row to the order table
        public Order PostOrder([FromBody] OrderJSON order) => _dataContext.AddToOrder(order);

        [HttpGet, Route("api/orderDetail/OrderId/{id}")]
        // gets all orders from order table
        public IEnumerable<OrderDetail> GetOrdersDetails(int id) => _dataContext.OrderDetails.Include("Product.Category").Where(o => o.OrderId == id );

        [HttpGet, Route("api/lastOrder/customer/{id}")]
        // gets all orders from order table
        public IEnumerable<Order> GetLastOrder(int id) => _dataContext.Orders.Include("OrderDetails").Where(o => o.CustomerId == id).OrderByDescending(o => o.OrderId).Take(1);

        [HttpPost, Route("api/addtoorderdetails")]
        // adds a row to the order table
        public OrderDetail PostOrderDetails([FromBody] OrderDetailJSON orderDetail) => _dataContext.AddToOrderDetails(orderDetail);

        [HttpDelete, Route("api/deleteallcartitem/CustomerId/{id}")]
        // deletes a row from the cartitem table
        public CartItem DeleteAllCartItem(int id) => _dataContext.DeleteAllCartItems(id);
    }
}