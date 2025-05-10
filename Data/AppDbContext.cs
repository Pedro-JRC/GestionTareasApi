using GestionTareasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionTareasApi.Data;

public class AppDbContext : DbContext
{
    // CONSTRUCTOR DEL CONTEXTO DE BASE DE DATOS
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #region DBSETS

    // TABLA DE TAREAS GENERALES
    public DbSet<TareaGeneral> Tareas { get; set; }

    // TABLA DE USUARIOS
    public DbSet<UsuariosModel> Usuarios { get; set; }

    #endregion

    #region CONFIGURACIÓN DE MODELOS

    /// <summary>
    /// CONFIGURA LAS ENTIDADES Y SUS TABLAS EN LA BASE DE DATOS
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // MAPEAR LA ENTIDAD TAREA A LA TABLA "Tareas"
        modelBuilder.Entity<TareaGeneral>().ToTable("Tareas");

        // MAPEAR LA ENTIDAD USUARIO A LA TABLA "Usuarios"
        modelBuilder.Entity<UsuariosModel>().ToTable("Usuarios");
    }

    #endregion
}
