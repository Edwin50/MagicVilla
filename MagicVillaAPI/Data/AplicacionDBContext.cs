using MagicVillaAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace MagicVillaAPI.Data
{
    public class AplicacionDBContext : DbContext
    {

        public AplicacionDBContext(DbContextOptions<AplicacionDBContext> options): base(options) 
        {
            
        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa() { 
                Id = 1,
                Nombre="Villa Real",
                Detalle="Ubicado en Villafranca",
                ImagenURL="",
                Tarifa=200,
                FechaActualizacion=DateTime.Now,
                FechaCreacion=DateTime.Now
               
                }
                
                );
        }

    }
}
