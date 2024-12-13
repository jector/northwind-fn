public class DiscountCartModel
{
    public IEnumerable<CartItem> CartItems { get; set; }
    public IEnumerable<Discount> Discounts { get; set; }
    public IEnumerable<Customer> Customers { get; set; }
    public IEnumerable<Order> Orders { get; set; }
    //public IEnumerable<OrderDetail> OrderDetails { get; set; }
}