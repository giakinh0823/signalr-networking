using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Q2.Models
{
    public partial class PE_PRN_Fall22B1Context : DbContext
    {
        public PE_PRN_Fall22B1Context()
        {
        }

        public PE_PRN_Fall22B1Context(DbContextOptions<PE_PRN_Fall22B1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Director> Directors { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<Producer> Producers { get; set; } = null!;
        public virtual DbSet<Star> Stars { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;database=PE_PRN_Fall22B1;Integrated security=true;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Director>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.FullName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Nationality)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.Title)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Language)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ReleaseDate).HasColumnType("date");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Director)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.DirectorId)
                    .HasConstraintName("FK_Movies_Directors");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.ProducerId)
                    .HasConstraintName("FK_Movies_Producers");

                entity.HasMany(d => d.Genres)
                    .WithMany(p => p.Movies)
                    .UsingEntity<Dictionary<string, object>>(
                        "MovieGenre",
                        l => l.HasOne<Genre>().WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Movie_Genre_Genres"),
                        r => r.HasOne<Movie>().WithMany().HasForeignKey("MovieId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Movie_Genre_Movies"),
                        j =>
                        {
                            j.HasKey("MovieId", "GenreId");

                            j.ToTable("Movie_Genre");
                        });

                entity.HasMany(d => d.Stars)
                    .WithMany(p => p.Movies)
                    .UsingEntity<Dictionary<string, object>>(
                        "MovieStar",
                        l => l.HasOne<Star>().WithMany().HasForeignKey("StarId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Movie_Star_Stars"),
                        r => r.HasOne<Movie>().WithMany().HasForeignKey("MovieId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Movie_Star_Movies"),
                        j =>
                        {
                            j.HasKey("MovieId", "StarId");

                            j.ToTable("Movie_Star");
                        });
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Star>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nationality)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
