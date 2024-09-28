using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace STP.Repository.Models;

public partial class ShareTaxiContext : DbContext
{
    public ShareTaxiContext()
    {
    }

    public ShareTaxiContext(DbContextOptions<ShareTaxiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<CarTrip> CarTrips { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Pricing> Pricings { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=ADMIN-PC\\MSSQLSERVER1;Initial Catalog=Share_Taxi;Persist Security Info=True;User ID=sa;Password=12345;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Area__3214EC07AAF2535E");

            entity.ToTable("Area");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC0719D2871E");

            entity.ToTable("Booking");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Trip).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("FK__Booking__TripId__35BCFE0A");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Booking__UserId__36B12243");
        });

        modelBuilder.Entity<CarTrip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarTrip__3214EC07E139768C");

            entity.ToTable("CarTrip");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DriverName).HasMaxLength(255);
            entity.Property(e => e.DriverPhone).HasMaxLength(255);
            entity.Property(e => e.PlateNumber).HasMaxLength(255);

            entity.HasOne(d => d.Trip).WithMany(p => p.CarTrips)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("FK__CarTrip__TripId__37A5467C");
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Deposit__3214EC07403D6C67");

            entity.ToTable("Deposit");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DepositDate).HasColumnType("datetime");
            entity.Property(e => e.DepositMethod).HasMaxLength(255);

            entity.HasOne(d => d.Transaction).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK__Deposit__Transac__38996AB5");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK__Deposit__WalletI__398D8EEE");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC270A8845DF");

            entity.ToTable("Location");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Lat).HasColumnType("decimal(10, 8)");
            entity.Property(e => e.Lon).HasColumnType("decimal(11, 8)");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Area).WithMany(p => p.Locations)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK__Location__AreaId__3A81B327");
        });

        modelBuilder.Entity<Pricing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pricing__3214EC071C6F3D43");

            entity.ToTable("Pricing");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Currency).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.DropOffLocation).WithMany(p => p.PricingDropOffLocations)
                .HasForeignKey(d => d.DropOffLocationId)
                .HasConstraintName("FK__Pricing__DropOff__3B75D760");

            entity.HasOne(d => d.PickUpLocation).WithMany(p => p.PricingPickUpLocations)
                .HasForeignKey(d => d.PickUpLocationId)
                .HasConstraintName("FK__Pricing__PickUpL__3C69FB99");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC077164203C");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ReferenceId).HasMaxLength(255);
            entity.Property(e => e.TransactionType).HasMaxLength(255);

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK__Transacti__Walle__3D5E1FD2");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Trip__3214EC07C4EB0600");

            entity.ToTable("Trip");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.DropOffLocationId).HasColumnName("dropOffLocationId");
            entity.Property(e => e.PickUpLocationId).HasColumnName("pickUpLocationId");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.DropOffLocation).WithMany(p => p.TripDropOffLocations)
                .HasForeignKey(d => d.DropOffLocationId)
                .HasConstraintName("FK__Trip__dropOffLoc__3E52440B");

            entity.HasOne(d => d.PickUpLocation).WithMany(p => p.TripPickUpLocations)
                .HasForeignKey(d => d.PickUpLocationId)
                .HasConstraintName("FK__Trip__pickUpLoca__3F466844");

            entity.HasOne(d => d.Pricing).WithMany(p => p.Trips)
                .HasForeignKey(d => d.PricingId)
                .HasConstraintName("FK__Trip__PricingId__403A8C7D");

            entity.HasOne(d => d.ToArea).WithMany(p => p.Trips)
                .HasForeignKey(d => d.ToAreaId)
                .HasConstraintName("FK__Trip__ToAreaId__412EB0B6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC075541214B");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(255);
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC072A2A5CD3");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CurrencyCode).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Wallet__UserId__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
