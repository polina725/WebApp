using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;
using static System.IO.File;


namespace XUnitTests
{
    class SeedInMemoryDatabase
    {
        private static string sourceFile = "sourceData/UsersWithOnePhoto.json";

        public static async void SeedUsersWithOnePhoto(DataContext context)
        {
            var userData = await ReadAllTextAsync(sourceFile);

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            context.AddRange(users);
            context.SaveChanges();
        }

        public static DbContextOptions<DataContext> GetOptions()
        {
            return new DbContextOptionsBuilder<DataContext>()
                 .UseInMemoryDatabase(databaseName: "InMemoryDataBase")
                 .Options;
        }
    }
}
