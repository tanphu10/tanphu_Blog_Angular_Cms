namespace TPBlog.Core.Domain.Entity
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        public TKey Id { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateLastModified { get; set; }
    }
}
