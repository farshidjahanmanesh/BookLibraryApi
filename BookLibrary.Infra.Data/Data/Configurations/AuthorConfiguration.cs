using BookLibrary.Domain.Domains.Authors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrary.Infra.Data.Data.Configurations
{
    class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(c => c.WikipediaUrl)
                .HasMaxLength(500);
            builder.Property(c => c.OpenLibraryId)
               .HasMaxLength(100)
               .IsRequired();
            builder.HasMany(c => c.Books)
                .WithMany(c => c.Authors);
        }
    }
}
