using Microsoft.EntityFrameworkCore;

namespace RagWebApi.DataContext
{
    public static class RagContextInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider) {

            using var context = new RagContext(serviceProvider.GetRequiredService<DbContextOptions<RagContext>>()); 
        }
    }
}
