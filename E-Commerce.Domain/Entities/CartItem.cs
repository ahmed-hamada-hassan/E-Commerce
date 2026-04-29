namespace E_Commerce.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }

    public CartItem() { }

    public static CartItem Create(Guid productId, string name, decimal price, int quantity, string? imageUrl)
    {
        return new CartItem
        {
            ProductId = productId,
            ProductName = name,
            UnitPrice = price,
            Quantity = quantity,
            ImageUrl = imageUrl
        };
    }
}
