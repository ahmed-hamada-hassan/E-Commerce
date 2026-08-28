namespace E_Commerce.Domain.Entities;

public class Cart
{
    public Guid Id { get; init; }
    public Guid? UserId { get; set; }
    public List<CartItem> Items { get; set; } = new();

    public Cart() { }

    public Cart(Guid id, Guid? userId)
    {
        Id = id;
        UserId = userId;
    }

    public decimal TotalPrice => Items.Sum(item => item.UnitPrice * item.Quantity);
}
