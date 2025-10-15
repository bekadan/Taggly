using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taggly.UrlShortener.Domain.Entities;

namespace Taggly.UrlShortener.Infrastructure.Configurations;

public class ShortUrlConfigurations : IEntityTypeConfiguration<ShortUrl>
{
    public void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        builder.ToTable("ShortUrls");

        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.ShortCode, sc =>
        {
            sc.WithOwner();

            sc.Property(p => p.Value)
              .HasColumnName("ShortCode")
              .IsRequired()
              .HasMaxLength(7);
        });

        builder.OwnsOne(x => x.OriginalUrl, ou =>
        {
            ou.WithOwner();

            ou.Property(p => p.Value)
              .HasColumnName("OriginalUrl")
              .IsRequired()
              .HasMaxLength(2048);
        });

        builder.OwnsOne(su => su.Metadata, md =>
        {
            md.WithOwner();

            md.Property(p => p.CreatedBy)
              .HasColumnName("CreatedBy")
              .HasMaxLength(100);

            md.Property(p => p.ExpirationDate)
              .HasColumnName("ExpirationDate");

            md.Property(p => p.Description)
              .HasColumnName("Description")
              .HasMaxLength(500);
        });

        builder.Property(su => su.CreatedAt)
               .IsRequired();

        builder.Property(su => su.ExpirationDate);

        builder.HasIndex(su => su.ShortCode.Value).IsUnique();
    }
}
