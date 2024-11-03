using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppCore.Data;

public class BaseEntity
{
    public Guid Id { get; set; }
    public ulong No { get; set; }
    public Guid? CreatorId { get; set; }
    public Guid? EditorId { get; set; }
    public Guid? DeleterId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime EditedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsTesting { get; set; }
    public byte[] RowVersion { get; set; }
}

public abstract class BaseConfig<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.No).ValueGeneratedOnAdd().HasColumnType("Bigint").IsRequired();
        builder.Property(x => x.Id).HasDefaultValueSql("newid()").IsRequired();
        builder.Property(x => x.CreatorId).IsRequired(false);
        builder.Property(x => x.EditorId).IsRequired(false);
        builder.Property(x => x.DeleterId).IsRequired(false);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.EditedAt).IsRequired();
        builder.Property(x => x.DeletedAt).IsRequired(false);
        builder.Property(x => x.IsDeleted).HasDefaultValue(false).IsRequired();
        builder.Property(x => x.IsTesting).HasDefaultValue(false).IsRequired();
        builder.Property(e => e.RowVersion).IsRowVersion();

        builder.HasIndex(x => x.CreatorId);
        builder.HasIndex(x => x.EditorId);
        builder.HasIndex(x => x.DeleterId);
    }
}