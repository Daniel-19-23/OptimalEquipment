using OptimalEquipment.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Contexts
{
    public class ConnectionSQLServer : DbContext
    {
        // Conecto mi base de datos y tablas correspondientes
        public ConnectionSQLServer(DbContextOptions<ConnectionSQLServer> options) : base(options) { }
        public DbSet<Equipment> Equipments { get; set; } // Equipos
        public DbSet<Climbing> Climbings { get; set; } // Escalada
    }
}
