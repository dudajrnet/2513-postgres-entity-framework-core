using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poststore.Models;

namespace PostStore.Data.Mappings;

public class CategoryMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder
            .HasKey(x => x.Id)
            .HasName("pk_category");

        builder
           .Property(x => x.Id)
           .HasColumnName("id")
           .UseIdentityAlwaysColumn()
           .UseSerialColumn();

        builder.OwnsOne(Product => Product.Heading, Heading =>
        {
           Heading.Property(x => x.Title)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(160)
                .HasColumnName("title");

           Heading.Property(x => x.Slug)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(160)
                .HasColumnName("slug");

           Heading.HasIndex(x => x.Slug)
                .IsUnique()
                .HasDatabaseName("idx_categories_slug");
        });

        builder.HasMany(x => x.Products);
    }
}