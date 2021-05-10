using BookLibrary.Domain.Domains.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Infra.Data.Data
{

    public class BookLibraryDbContext : IdentityDbContext<User>
    {
        public BookLibraryDbContext(DbContextOptions<BookLibraryDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookLibraryDbContext).Assembly);
        }

    }
}
