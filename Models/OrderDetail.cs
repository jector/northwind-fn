using System.ComponentModel.DataAnnotations.Schema;
public class OrderDetail
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    [Column(TypeName = "decimal(18,4)")]
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(4,4)")]
    public decimal Discount { get; set; }
    public Product Product { get; set; }
    public ICollection<Order> Orders { get; set; }
}
