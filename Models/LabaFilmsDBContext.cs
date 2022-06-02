using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laba2FilmsBD.Models
{
    public partial class LabaFilmsDBContext : DbContext
    {
        public LabaFilmsDBContext()
        {
        }

        public LabaFilmsDBContext(DbContextOptions<LabaFilmsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; } = null!;
        public virtual DbSet<ActorsInFilm> ActorsInFilms { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Film> Films { get; set; } = null!;
        public virtual DbSet<Purchase> Purchases { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= DESKTOP-QO2SGDT; Database=LabaFilmsDB; Trusted_Connection=True; TrustServerCertificate=True; MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.Property(e => e.BirthDay).HasColumnType("date");

                entity.Property(e => e.Gender).HasMaxLength(50);
            });

            modelBuilder.Entity<ActorsInFilm>(entity =>
            {
                entity.HasKey(e => new { e.ActorId, e.FilmId });

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.ActorsInFilms)
                    .HasForeignKey(d => d.ActorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActorsInFilms_Actors");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.ActorsInFilms)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActorsInFilms_Films");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.LastActiveDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Last_Active_Date");
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.Property(e => e.Genre).HasMaxLength(50);

                entity.Property(e => e.Language).HasMaxLength(50);

                entity.Property(e => e.ReleaseYear).HasColumnName("Release_year");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasKey(e => new { e.FilmId, e.CustomerId });

                entity.Property(e => e.PaymentDay)
                    .HasColumnType("datetime")
                    .HasColumnName("Payment_day");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchases_Customers");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchases_Films");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
