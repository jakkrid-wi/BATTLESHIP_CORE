using BATTLESHIP_CORE_EF;
using Microsoft.EntityFrameworkCore;

namespace BATTLESHIP_CORE_TEST
{
    public static class DbContextMocker
    {
        public static DBContext GetDbContext(string dbName)
        {
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            // Create instance of DbContext
            var dbContext = new DBContext(options);

            return dbContext;
        }
    }

}
