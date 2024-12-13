using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
  public DataContext(DbContextOptions<DataContext> options) : base(options) { }

  public DbSet<Product> Products { get; set; }
  public DbSet<Category> Categories { get; set; }
  public DbSet<Discount> Discounts { get; set; }
  public DbSet<Customer> Customers { get; set; }
  public DbSet<CartItem> CartItems { get; set; }
  public DbSet<Order> Orders { get; set; }
  public DbSet<OrderDetail> OrderDetails { get; set; }

  public void AddCustomer(Customer customer)
  {
    Customers.Add(customer);
    SaveChanges();
  }
  public void EditCustomer(Customer customer)
  {
    var customerToUpdate = Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
    customerToUpdate.Address = customer.Address;
    customerToUpdate.City = customer.City;
    customerToUpdate.Region = customer.Region;
    customerToUpdate.PostalCode = customer.PostalCode;
    customerToUpdate.Country = customer.Country;
    customerToUpdate.Phone = customer.Phone;
    customerToUpdate.Fax = customer.Fax;
    SaveChanges();
  }
  public CartItem AddToCart(CartItemJSON cartItemJSON)
  {
    int CustomerId = Customers.FirstOrDefault(c => c.Email == cartItemJSON.email).CustomerId;
    int ProductId = cartItemJSON.id;
    // check for duplicate cart item
    CartItem cartItem = CartItems.FirstOrDefault(ci => ci.ProductId == ProductId && ci.CustomerId == CustomerId);
    if (cartItem == null)
    {
      // this is a new cart item
      cartItem = new CartItem()
      {
        CustomerId = CustomerId,
        ProductId = cartItemJSON.id,
        Quantity = cartItemJSON.qty
      };
      CartItems.Add(cartItem);
    }
    else
    {
      // for duplicate cart item, simply update the quantity
      cartItem.Quantity += cartItemJSON.qty;
    }
    SaveChanges();
    cartItem.Product = Products.Find(cartItem.ProductId);
    return cartItem;
  }
  public CartItem DeleteCartItems(int id)
  {
      CartItem cartItem = CartItems.FirstOrDefault(ci => ci.CartItemId == id);
      if (cartItem != null)
      {
          CartItems.Remove(cartItem);
          SaveChanges();
          return cartItem;
      }
      return null;
  }

  public Order AddToOrder(OrderJSON orderJSON)
  {
    
    Order order = new Order()
    {
      CustomerId = orderJSON.customerId,
      EmployeeId = orderJSON.employeeId,
      OrderDate = orderJSON.orderDate,
      RequiredDate = orderJSON.requiredDate,
      ShippedDate = orderJSON.shippedDate,
      ShipVia = orderJSON.shipVia,
      Freight = orderJSON.freight,
      ShipName = orderJSON.shipName,
      ShipAddress = orderJSON.shipAddress,
      ShipCity = orderJSON.shipCity,
      ShipRegion = orderJSON.shipRegion,
      ShipPostalCode = orderJSON.shipPostalCode,
      ShipCountry = orderJSON.shipCountry
    };
    Orders.Add(order);
    SaveChanges();
    order.Customer = Customers.Find(order.CustomerId);
    return order;
  }

  public OrderDetail AddToOrderDetails(OrderDetailJSON orderDetailJSON)
  {
    OrderDetail orderDetail = new OrderDetail()
    {
      OrderId = orderDetailJSON.OrderId,
      ProductId = orderDetailJSON.ProductId,
      UnitPrice = orderDetailJSON.UnitPrice,
      Quantity = orderDetailJSON.Quantity,
      Discount = orderDetailJSON.Discount
    };
    OrderDetails.Add(orderDetail);
    SaveChanges();
    return orderDetail;
  }

 public CartItem DeleteAllCartItems(int id)
  {
      CartItem cartItem = CartItems.Where(ci => ci.CustomerId == id).FirstOrDefault();

      foreach (var item in CartItems.Where(ci => ci.CustomerId == id))
      {
          CartItems.Remove(item);
      }
      SaveChanges();
      return cartItem; 

  }

}
