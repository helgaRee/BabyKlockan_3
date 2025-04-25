using BabyKlockan_3.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BabyKlockanTest.TestHelpers;

public static class DbContextHelper
{
    public static DataContext GetInMemoryDbContext()
    {
        //skapa en temporär SQLite-ansultning i minnet
        var connection = new SqliteConnection("DataSource=:memory:"); //:memory gör att SQLite inte skapar en riktig fil på disk
        connection.Open();

        //konfig EF att använda den anslutnigen
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(connection)
            .Options;

        var context = new DataContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}
