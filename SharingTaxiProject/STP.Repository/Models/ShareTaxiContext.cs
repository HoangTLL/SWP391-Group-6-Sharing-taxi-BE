// Namespace chứa các model và DbContext
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace STP.Repository.Models
{
    // DbContext dùng để giao tiếp với cơ sở dữ liệu
    public partial class ShareTaxiContext : DbContext
    {
        // Constructor mặc định của DbContext
        public ShareTaxiContext()
        {
        }

        // Constructor nhận vào các tùy chọn cấu hình cho DbContext
        public ShareTaxiContext(DbContextOptions<ShareTaxiContext> options)
            : base(options)
        {
        }

        // Khai báo các DbSet tương ứng với các bảng trong cơ sở dữ liệu
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<CarTrip> CarTrips { get; set; }
        public virtual DbSet<Deposit> Deposits { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }

        // Thêm DbSet cho hai bảng TripType và TripeTypePricing
        public virtual DbSet<TripType> TripTypes { get; set; }
        public virtual DbSet<TripTypePricing> TripTypePricings { get; set; }


        // Lấy chuỗi kết nối từ file cấu hình (appsettings.json)
        public static string GetConnectionString(string connectionStringName)
        {
            // Tạo cấu hình từ file appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            // Lấy chuỗi kết nối từ tên chuỗi đã cho
            string connectionString = config.GetConnectionString(connectionStringName);
            return connectionString;
        }

        // Phương thức cấu hình DbContext với chuỗi kết nối
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));

        // Phương thức thiết lập cấu hình của các bảng trong cơ sở dữ liệu
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình bảng Area
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Area__3214EC07AAF2535E");
                entity.ToTable("Area");
                entity.Property(e => e.Id).UseIdentityColumn(); // Giá trị Id không tự động tăng
                entity.Property(e => e.Description).HasMaxLength(255); // Mô tả dài tối đa 255 ký tự
                entity.Property(e => e.Name).HasMaxLength(255); // Tên dài tối đa 255 ký tự
            });

            // Cấu hình bảng Booking
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC0719D2871E");
                entity.ToTable("Booking");
                entity.Property(e => e.Id).UseIdentityColumn();

                // Thiết lập quan hệ với bảng Trip
                entity.HasOne(d => d.Trip).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.TripId)
                    .HasConstraintName("FK__Booking__TripId__35BCFE0A");

                // Thiết lập quan hệ với bảng User
                entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Booking__UserId__36B12243");
            });

            // Cấu hình bảng CarTrip
            modelBuilder.Entity<CarTrip>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CarTrip__3214EC07E139768C");
                entity.ToTable("CarTrip");
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.DriverName).HasMaxLength(255); // Tên tài xế
                entity.Property(e => e.DriverPhone).HasMaxLength(255); // Số điện thoại tài xế
                entity.Property(e => e.PlateNumber).HasMaxLength(255); // Biển số xe

                // Thiết lập quan hệ với bảng Trip
                entity.HasOne(d => d.Trip).WithMany(p => p.CarTrips)
                    .HasForeignKey(d => d.TripId)
                    .HasConstraintName("FK__CarTrip__TripId__37A5467C");
            });

            // Cấu hình bảng Deposit (khoản tiền gửi)
            modelBuilder.Entity<Deposit>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Deposit__3214EC07403D6C67");
                entity.ToTable("Deposit");
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)"); // Số tiền có 2 chữ số thập phân
                entity.Property(e => e.DepositDate).HasColumnType("datetime"); // Ngày gửi
                entity.Property(e => e.DepositMethod).HasMaxLength(255); // Phương thức gửi

                // Quan hệ 1-1 giữa Transaction và Deposit
                modelBuilder.Entity<Transaction>()
                    .HasOne(t => t.Deposit)         // Mỗi Transaction có một Deposit
                    .WithOne(d => d.Transaction)    // Mỗi Deposit có một Transaction
                    .HasForeignKey<Transaction>(t => t.DepositId)  // Khóa ngoại ở bảng Transaction
                    .HasConstraintName("FK_Transaction_Deposit");

                // Thiết lập quan hệ với bảng Wallet (ví)
                entity.HasOne(d => d.Wallet).WithMany(p => p.Deposits)
                    .HasForeignKey(d => d.WalletId)
                    .HasConstraintName("FK__Deposit__WalletI__398D8EEE");
            });

            // Cấu hình bảng Location (vị trí)
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Location__3214EC270A8845DF");
                entity.ToTable("Location");
                entity.Property(e => e.Id)
                    .UseIdentityColumn()
                    .HasColumnName("ID");
                entity.Property(e => e.Lat).HasColumnType("decimal(10, 8)"); // Vĩ độ
                entity.Property(e => e.Lon).HasColumnType("decimal(11, 8)"); // Kinh độ
                entity.Property(e => e.Name).HasMaxLength(255); // Tên vị trí

                // Thiết lập quan hệ với bảng Area
                entity.HasOne(d => d.Area).WithMany(p => p.Locations)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK__Location__AreaId__3A81B327");
            });

            // Cấu hình bảng Transaction (giao dịch)
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC077164203C");
                entity.ToTable("Transaction");
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)"); // Số tiền giao dịch
                entity.Property(e => e.CreatedAt).HasColumnType("datetime"); // Thời gian giao dịch
                entity.Property(e => e.ReferenceId).HasMaxLength(255); // Mã tham chiếu
                entity.Property(e => e.TransactionType).HasMaxLength(255); // Loại giao dịch

                // Thiết lập quan hệ với bảng Wallet (ví)
                entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.WalletId)
                    .HasConstraintName("FK__Transacti__Walle__3D5E1FD2");
            });

            // Cấu hình bảng Trip (chuyến đi)
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Trip__3214EC07C4EB0600");
                entity.ToTable("Trip");
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.BookingDate).HasColumnType("dateonly"); // Ngày đặt
                entity.Property(e => e.DropOffLocationId).HasColumnName("dropOffLocationId");
                entity.Property(e => e.PickUpLocationId).HasColumnName("pickUpLocationId");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)"); // Giá mỗi chuyến

                // Quan hệ với bảng Location (địa điểm trả khách)
                entity.HasOne(d => d.DropOffLocation).WithMany(p => p.TripDropOffLocations)
                    .HasForeignKey(d => d.DropOffLocationId)
                    .HasConstraintName("FK__Trip__dropOffLoc__3E52440B");

                // Quan hệ với bảng Location (địa điểm đón khách)
                entity.HasOne(d => d.PickUpLocation).WithMany(p => p.TripPickUpLocations)
                    .HasForeignKey(d => d.PickUpLocationId)
                    .HasConstraintName("FK__Trip__pickUpLoca__3F466844");

                // Quan hệ với bảng ToArea
                entity.HasOne(d => d.ToArea).WithMany(p => p.Trips)
                    .HasForeignKey(d => d.ToAreaId)
                    .HasConstraintName("FK__Trip__ToAreaId__412EB0B6");
            });

            // Cấu hình bảng User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__User__3214EC075541214B");
                entity.ToTable("User");
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime"); // Ngày tạo tài khoản
                entity.Property(e => e.Email).HasMaxLength(255); // Email người dùng
                entity.Property(e => e.Name).HasMaxLength(255); // Tên người dùng
                entity.Property(e => e.Password).HasMaxLength(255); // Mật khẩu người dùng
                entity.Property(e => e.PhoneNumber).HasMaxLength(255); // Số điện thoại
                entity.Property(e => e.Role).HasMaxLength(255); // Vai trò người dùng
            });

            // Cấu hình bảng Wallet (ví)
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC072A2A5CD3");
                entity.ToTable("Wallet");
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)"); // Số dư trong ví
                entity.Property(e => e.CreatedAt).HasColumnType("datetime"); // Ngày tạo ví
                entity.Property(e => e.CurrencyCode).HasMaxLength(255); // Mã tiền tệ
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime"); // Ngày cập nhật ví

                // Quan hệ với bảng User
                entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Wallet__UserId__4222D4EF");
            });

            // Cấu hình hai bảng mới TripType và TripeTypePricing

            modelBuilder.Entity<TripType>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__TripType__3214EC07");
                entity.ToTable("TripType");

                // Thuộc tính cho bảng TripType
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.BasicePrice).HasColumnType("decimal(10, 2)");

                // Quan hệ 1-n với bảng Trip
                entity.HasMany(d => d.Trips)
                    .WithOne(p => p.TripType)
                    .HasForeignKey(p => p.TripTypeId)
                    .HasConstraintName("FK__Trip__TripTypeId");

                // Quan hệ 1-n với bảng TripeTypePricing
                entity.HasMany(d => d.TripTypePricings)
                    .WithOne(p => p.TripTypeNavigation)
                    .HasForeignKey(p => p.TripType)
                    .HasConstraintName("FK__TripeTypePricing__TripType");
            });

            modelBuilder.Entity<TripTypePricing>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__TripeTypePricing__3214EC07");
                entity.ToTable("TripeTypePricing");

                // Thuộc tính cho bảng TripeTypePricing
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.PricePerPerson).HasColumnType("decimal(10, 2)");

                // Quan hệ với bảng TripType
                entity.HasOne(d => d.TripTypeNavigation)
                    .WithMany(p => p.TripTypePricings)
                    .HasForeignKey(d => d.TripType)
                    .HasConstraintName("FK__TripeTypePricing__TripType");
            });

            // Đặt thêm các cấu hình nếu cần
            OnModelCreatingPartial(modelBuilder);
        }

        // Partial method có thể được ghi đè ở file khác nếu có các cấu hình bổ sung
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
