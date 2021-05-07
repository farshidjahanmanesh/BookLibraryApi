using BookLibrary.Domain.Domains.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Infra.Data.Data.Configurations
{
    class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Title).HasMaxLength(500)
                .IsRequired();
            builder.HasIndex(c => c.Title);
            builder.Property(c => c.OpenLibraryId)
                .HasMaxLength(100)
                .IsRequired();
            builder.HasMany(c => c.Authors)
                .WithMany(c => c.Books);

        }
    }
}
