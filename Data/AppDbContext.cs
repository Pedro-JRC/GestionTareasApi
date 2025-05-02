using GestionTareasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionTareasApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #region DBSETS

    public DbSet<TareaGeneral> Tareas { get; set; }

    public DbSet<UsuariosModel> Usuarios { get; set; }

    #endregion

    #region CONFIGURACIÓN DE MODELOS

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TareaGeneral>().ToTable("Tareas");
        modelBuilder.Entity<UsuariosModel>().ToTable("Usuarios");
    }

    #endregion
}
