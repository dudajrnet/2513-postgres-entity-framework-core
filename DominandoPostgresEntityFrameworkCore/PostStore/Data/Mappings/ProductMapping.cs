using System.Text.Json;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poststore.Models;
using PostStore.Models;

namespace Poststore.Data.Mapping;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower  
    };

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder
            .HasKey(x=> x.Id)
            .HasName("pk_product");

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn()
            .UseSerialColumn();

        builder.OwnsOne(Product => Product.Heading, Heading =>
        {
          Heading.Property(x=> x.Title)
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(160)
            .HasColumnName("title");

          Heading.Property(x=> x.Slug)
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(160)
            .HasColumnName("slug");

          Heading.HasIndex(x=> x.Slug)
            .IsUnique()
            .HasDatabaseName("idx_products_slug");
        } );        

        builder.Property(x=> x.CategoryId)                                   
            .HasColumnName("category_id");
        
        builder.HasOne(x=> x.Category);
                
        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired(true);                            

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .IsRequired(true);

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired(true);

        builder.Property(product => product.DefaultLanguage )
            .HasMaxLength(8)
            .HasDefaultValue("en-us")
            .HasColumnName("default_language")
            .IsRequired();

        builder.Property(product => product.Translations)
            .HasColumnName("translation")
            .HasColumnType("jsonb")
            .HasConversion(
                translations => JsonSerializer.Serialize(translations, _jsonOptions),
                String => string.IsNullOrEmpty(String) ? new List<Translation>()
                : JsonSerializer.Deserialize<List<Translation>>(String,_jsonOptions)
                    ?? new List<Translation>());
    }
}