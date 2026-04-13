namespace E_Commerce.Domain.Common
{
    public abstract class SoftDeletable : ISoftDeletable
    {
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeleteOn { get; set; }

        public virtual void Restore()
        {
            this.IsDeleted = false;
            this.DeleteOn = null;
        }

        public void Delete()
        {
            this.IsDeleted = true;
            this.DeleteOn = DateTimeOffset.UtcNow;
        }
    }
}
