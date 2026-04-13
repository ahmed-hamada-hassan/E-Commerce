namespace E_Commerce.Domain.Common;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeleteOn { get; set; }
    public void Restore();
    public void Delete();
}
