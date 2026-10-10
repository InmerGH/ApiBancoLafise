using BancoApi.Models;
using Microsoft.EntityFrameworkCore;
namespace BancoApi.Data
{
    public class BancoDbContext : DbContext
    {
        public BancoDbContext(DbContextOptions<BancoDbContext> options) : base(options) { }
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<CuentaBancaria> Cuentas => Set<CuentaBancaria>();
        public DbSet<Transaccion> Transacciones => Set<Transaccion>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CuentaBancaria>()
            .HasIndex(c => c.NumeroCuenta)
            .IsUnique();
        }
    }
}
