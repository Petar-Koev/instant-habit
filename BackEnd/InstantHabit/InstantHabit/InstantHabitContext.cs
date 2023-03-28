using InstantHabit.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System;


namespace InstantHabit
{
    public partial class InstantHabitContext: DbContext
    {
        public InstantHabitContext()
        {

        }
        public InstantHabitContext(DbContextOptions<InstantHabitContext> options): base(options)
        {

        }
        public virtual DbSet<Habit> Habits { get; set; }
        public virtual DbSet<Day> Days { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-4858O2V;Database=InstantHabit;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Habit>(entity =>
            {
                entity.ToTable("Habits", "InstantHabit");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Description)
                .IsRequired(false)
                    .HasMaxLength(500);

                entity.Property(e => e.IsExtended)
               .IsRequired();
            });

            modelBuilder.Entity<Day>(entity =>
            {
                entity.ToTable("Days", "InstantHabit");

                entity.Property(e => e.Note)
                .IsRequired(false)
                    .HasMaxLength(500);

                entity.Property(e => e.DayNumber)
                .IsRequired();


                entity.HasOne(d => d.Habit)
                    .WithMany(p => p.Day)
                    .HasForeignKey(d => d.HabitId)
                    .HasConstraintName("FK_HabitId");
            });

            OnModelCreatingPartial(modelBuilder);
        }
    }
}
