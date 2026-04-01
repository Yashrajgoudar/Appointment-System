namespace AuthService.Infrastructure.Persistence.Seeders
{
    public class DatabaseSeeder
    {
        public static async Task SeedAsync(AuthDbContext context)
        {
            await RoleSeeder.SeedRolesAsync(context);
        }
    }
}
