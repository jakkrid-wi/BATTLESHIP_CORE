using BATTLESHIP_CORE_EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_EF
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Board> Board { get; set; }
        public DbSet<ActionShipInstall> ActionShipInstall { get; set; }
        public DbSet<ActionMove> ActionMove { get; set; }
        public DbSet<Ship> Ship { get; set; }
    }
}
