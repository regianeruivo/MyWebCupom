using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyWebCupom.Models;

public partial class DbTechchallengeContext : DbContext
{
    public DbTechchallengeContext()
    {
    }

    public DbTechchallengeContext(DbContextOptions<DbTechchallengeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbCupom> TbCupoms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:db-techchallenge.database.windows.net,1433;Initial Catalog=db_techchallenge;Persist Security Info=False;User ID=ticketuser;Password=pass@2023;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbCupom>(entity =>
        {
            entity                
                .ToTable("TB_CUPOM");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UrlCupom)
                .HasMaxLength(70)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
