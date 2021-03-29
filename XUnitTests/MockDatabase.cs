using API.Data;
using API.Entities;
using System.Collections.Generic;
using System.Text.Json;
using static System.IO.File;


namespace XUnitTests
{
    class MockDatabase
    {
        private static string sourceFile = "UserSeedData.json";

        public static async void SeedData(DataContext context)
        {
            var userData = await ReadAllTextAsync(sourceFile);

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            context.AddRange(users);
            context.SaveChanges();
        }
    }
}
