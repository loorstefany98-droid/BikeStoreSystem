using Microsoft.EntityFrameworkCore;
using BikeStore.Datos.Models;

namespace BikeStore.Datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<BikeStore.Datos.Models.Bicicleta> Bicicletas { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // mapeo manual para asegurar nombres de tabla exactos si es necesario, 
            // aunque ya lo pusimos con DataAnnotations [Table(...)]
        }
    }
}
