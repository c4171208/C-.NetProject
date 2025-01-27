﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QueueManagmentDB.EF.Models;

namespace QueueManagmentDB.EF.Contexts;

public partial class QueueManagmentContext : DbContext
{
    public QueueManagmentContext()
    {
    }

    public QueueManagmentContext(DbContextOptions<QueueManagmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Queue> Queues { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PC-ASUS\\MYSQL;Initial Catalog=QUEUE_MANAGEMENT;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Queue>(entity =>
        {
            entity.HasKey(e => e.QueId).HasName("PK__QUEUE__5782944BA0E026DE");

            entity.ToTable("QUEUE");

            entity.Property(e => e.DesignatedTime).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Queues)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__QUEUE__UserId__48CFD27E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C0CFFF232");

            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(25);
            entity.Property(e => e.LastName).HasMaxLength(25);
            entity.Property(e => e.Password).HasMaxLength(35);
            entity.Property(e => e.Salt).HasMaxLength(64);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
