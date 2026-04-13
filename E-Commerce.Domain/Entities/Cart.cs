namespace E_Commerce.Domain.Entities;

public class Cart
{
    public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; } = new();

    public Cart() { }

    public Cart(Guid userId)
    {
        UserId = userId;
    }

    public decimal TotalPrice => Items.Sum(item => item.UnitPrice * item.Quantity);
}
