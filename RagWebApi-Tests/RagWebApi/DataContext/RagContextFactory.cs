using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Identity.Client;

namespace RagWebApi.DataContext
{
    public class RagContextFactory : IDesignTimeDbContextFactory<RagContext>
    {

        public RagContextFactory() { }
        public RagContext CreateDbContext(string[] args) { 
            var optionsBuilder = new DbContextOptionsBuilder<RagContext>();
            optionsBuilder.UseSqlServer("Server=TALPUR-PC;Database=RagContext;TrustServerCertificate=True;Encrypt=False;Trusted_Connection=True");
            return new RagContext(optionsBuilder.Options);

        }
    }
}
