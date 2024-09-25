USE [master]
GO
/****** Object:  Database [Share_Taxi]    Script Date: 25/09/2024 3:23:10 CH ******/
CREATE DATABASE [Share_Taxi]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Share_Taxi', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEPRESS\MSSQL\DATA\Share_Taxi.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Share_Taxi_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEPRESS\MSSQL\DATA\Share_Taxi_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Share_Taxi] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Share_Taxi].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Share_Taxi] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Share_Taxi] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Share_Taxi] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Share_Taxi] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Share_Taxi] SET ARITHABORT OFF 
GO
ALTER DATABASE [Share_Taxi] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Share_Taxi] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Share_Taxi] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Share_Taxi] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Share_Taxi] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Share_Taxi] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Share_Taxi] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Share_Taxi] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Share_Taxi] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Share_Taxi] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Share_Taxi] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Share_Taxi] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Share_Taxi] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Share_Taxi] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Share_Taxi] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Share_Taxi] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Share_Taxi] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Share_Taxi] SET RECOVERY FULL 
GO
ALTER DATABASE [Share_Taxi] SET  MULTI_USER 
GO
ALTER DATABASE [Share_Taxi] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Share_Taxi] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Share_Taxi] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Share_Taxi] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Share_Taxi] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Share_Taxi] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Share_Taxi', N'ON'
GO
ALTER DATABASE [Share_Taxi] SET QUERY_STORE = OFF
GO
USE [Share_Taxi]
GO
/****** Object:  Table [dbo].[Area]    Script Date: 25/09/2024 3:23:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Area](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 25/09/2024 3:23:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[Id] [int] NOT NULL,
	[UserId] [int] NULL,
	[TripId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CarTrip]    Script Date: 25/09/2024 3:23:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarTrip](
	[Id] [int] NOT NULL,
	[TripId] [int] NULL,
	[DriverName] [nvarchar](255) NULL,
	[DriverPhone] [nvarchar](255) NULL,
	[PlateNumber] [nvarchar](255) NULL,
	[ArrivedTime] [time](7) NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Deposit]    Script Date: 25/09/2024 3:23:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deposit](
	[Id] [int] NOT NULL,
	[WalletId] [int] NULL,
	[TransactionId] [int] NULL,
	[Amount] [decimal](10, 2) NULL,
	[DepositMethod] [nvarchar](255) NULL,
	[DepositDate] [datetime] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 25/09/2024 3:23:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Lat] [decimal](10, 8) NULL,
	[Lon] [decimal](11, 8) NULL,
	[AreaId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pricing]    Script Date: 25/09/2024 3:23:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pricing](
	[Id] [int] NOT NULL,
	[PickUpLocationId] [int] NULL,
	[DropOffLocationId] [int] NULL,
	[Price] [decimal](10, 2) NULL,
	[Currency] [nvarchar](255) NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 25/09/2024 3:23:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [int] NOT NULL,
	[WalletId] [int] NULL,
	[Amount] [decimal](10, 2) NULL,
	[TransactionType] [nvarchar](255) NULL,
	[ReferenceId] [nvarchar](255) NULL,
	[Status] [int] NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trip]    Script Date: 25/09/2024 3:23:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trip](
	[Id] [int] NOT NULL,
	[pickUpLocationId] [int] NULL,
	[dropOffLocationId] [int] NULL,
	[ToAreaId] [int] NULL,
	[MaxPerson] [int] NULL,
	[MinPerson] [int] NULL,
	[UnitPrice] [decimal](10, 2) NULL,
	[BookingDate] [datetime] NULL,
	[HourInDay] [time](7) NULL,
	[PricingId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 25/09/2024 3:23:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[DateOfBirth] [date] NULL,
	[CreatedAt] [datetime] NULL,
	[Role] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 25/09/2024 3:23:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[Id] [int] NOT NULL,
	[UserId] [int] NULL,
	[Balance] [decimal](10, 2) NULL,
	[CurrencyCode] [nvarchar](255) NULL,
	[Status] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Area] ([Id], [Name], [Description], [Status]) VALUES (1, N'VinHome', N'Khu dân cu Vinhome thu?c t?p doàn vin group g?m có 2 ph?n khu chính (Rainbow và Origami) ', 1)
INSERT [dbo].[Area] ([Id], [Name], [Description], [Status]) VALUES (2, N'Nhà Van Hóa Sinh Viên', N'Thu?c làng d?i h?c là khu h?c t?p dàng cho sinh viên kì % nghành IT c?a d?i h?c FPT', 1)
INSERT [dbo].[Area] ([Id], [Name], [Description], [Status]) VALUES (3, N'Ð?i h?c FPT', N'Là tru?ng d?i h?c thu?c khu công ngh? cao thành ph? Th? Ð?c', 1)
GO
INSERT [dbo].[Location] ([ID], [Name], [Lat], [Lon], [AreaId]) VALUES (1, N'Tòa S5.01 - Vinhome', CAST(21.03065300 AS Decimal(10, 8)), CAST(105.84713000 AS Decimal(11, 8)), 1)
INSERT [dbo].[Location] ([ID], [Name], [Lat], [Lon], [AreaId]) VALUES (2, N'Tòa S2.02 - Vinhome', CAST(21.03065200 AS Decimal(10, 8)), CAST(105.84823000 AS Decimal(11, 8)), 1)
INSERT [dbo].[Location] ([ID], [Name], [Lat], [Lon], [AreaId]) VALUES (3, N'Toà S6.01 - Vinhome', CAST(24.40065200 AS Decimal(10, 8)), CAST(135.82823000 AS Decimal(11, 8)), 1)
INSERT [dbo].[Location] ([ID], [Name], [Lat], [Lon], [AreaId]) VALUES (4, N'Toà S8.01 - Vinhome', CAST(30.40067000 AS Decimal(10, 8)), CAST(200.88923000 AS Decimal(11, 8)), 1)
INSERT [dbo].[Location] ([ID], [Name], [Lat], [Lon], [AreaId]) VALUES (5, N'Toà S10.01 - Vinhome', CAST(41.39165200 AS Decimal(10, 8)), CAST(245.64523000 AS Decimal(11, 8)), 1)
INSERT [dbo].[Location] ([ID], [Name], [Lat], [Lon], [AreaId]) VALUES (6, N'C?ng Chính - ÐH FPT', CAST(21.01203300 AS Decimal(10, 8)), CAST(105.52528400 AS Decimal(11, 8)), 3)
INSERT [dbo].[Location] ([ID], [Name], [Lat], [Lon], [AreaId]) VALUES (7, N'C?ng Ph? - ÐH FPT', CAST(24.40065200 AS Decimal(10, 8)), CAST(135.82823000 AS Decimal(11, 8)), 3)
INSERT [dbo].[Location] ([ID], [Name], [Lat], [Lon], [AreaId]) VALUES (8, N'C?ng Tru?c - NHVSV', CAST(10.76262200 AS Decimal(10, 8)), CAST(106.66017200 AS Decimal(11, 8)), 2)
INSERT [dbo].[Location] ([ID], [Name], [Lat], [Lon], [AreaId]) VALUES (9, N'C?ng Sau - NHVSV', CAST(10.84562200 AS Decimal(10, 8)), CAST(135.73452000 AS Decimal(11, 8)), 2)
GO
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (1, 1, 6, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (2, 1, 7, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (3, 1, 8, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (4, 1, 9, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (5, 2, 6, CAST(80.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (6, 2, 7, CAST(80.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (7, 2, 8, CAST(100.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (8, 2, 9, CAST(100.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (9, 3, 6, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (10, 3, 7, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (11, 3, 8, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (12, 3, 9, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (13, 4, 6, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (14, 4, 7, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (15, 4, 8, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (16, 4, 9, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (17, 5, 6, CAST(95.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (18, 5, 7, CAST(95.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (19, 5, 8, CAST(120.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (20, 5, 9, CAST(120.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (21, 6, 1, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (22, 7, 1, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (23, 8, 1, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (24, 9, 1, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (25, 6, 2, CAST(80.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (26, 7, 2, CAST(80.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (27, 8, 2, CAST(100.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (28, 9, 2, CAST(100.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (29, 6, 3, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (30, 7, 3, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (31, 8, 3, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (32, 9, 3, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (33, 6, 4, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (34, 7, 4, CAST(90.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (35, 8, 4, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (36, 9, 4, CAST(110.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (37, 6, 5, CAST(95.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (38, 7, 5, CAST(95.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (39, 8, 5, CAST(120.00 AS Decimal(10, 2)), N'VND', 1)
INSERT [dbo].[Pricing] ([Id], [PickUpLocationId], [DropOffLocationId], [Price], [Currency], [Status]) VALUES (40, 9, 5, CAST(120.00 AS Decimal(10, 2)), N'VND', 1)
GO
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role]) VALUES (1, N'Nguy?n Van Phúc', N'nguyenvanphuc@gmail.com', N'0868355460', N'Phucnv38', CAST(N'2004-03-08' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'admin')
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role]) VALUES (2, N'Tr?n Th? Dung', N'tranthidung@gmail.com', N'0912345679', N'Dungtt239', CAST(N'2004-08-25' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'staff')
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role]) VALUES (3, N'Lê Van Cu?ng', N'levancuong@gmail.com', N'0912345680', N'Cuonglv220', CAST(N'2002-02-20' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'user')
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role]) VALUES (4, N'Ph?m Th? Hoa', N'phamthihoa@gmail.com', N'0912345681', N'Hoapt1214', CAST(N'2003-12-14' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'user')
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role]) VALUES (5, N'Hoàng Minh Trí', N'hoangminhtri@gmail.com', N'0912345682', N'Trihm79', CAST(N'2004-07-09' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'user')
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role]) VALUES (6, N'Vu H?ng Phúc', N'vuhongphuc@gmail.com', N'0912345683', N'Phucvh330', CAST(N'2003-03-30' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'user')
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD FOREIGN KEY([TripId])
REFERENCES [dbo].[Trip] ([Id])
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[CarTrip]  WITH CHECK ADD FOREIGN KEY([TripId])
REFERENCES [dbo].[Trip] ([Id])
GO
ALTER TABLE [dbo].[Deposit]  WITH CHECK ADD FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transaction] ([Id])
GO
ALTER TABLE [dbo].[Deposit]  WITH CHECK ADD FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([Id])
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD FOREIGN KEY([AreaId])
REFERENCES [dbo].[Area] ([Id])
GO
ALTER TABLE [dbo].[Pricing]  WITH CHECK ADD FOREIGN KEY([DropOffLocationId])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[Pricing]  WITH CHECK ADD FOREIGN KEY([PickUpLocationId])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([Id])
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD FOREIGN KEY([dropOffLocationId])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD FOREIGN KEY([pickUpLocationId])
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD FOREIGN KEY([PricingId])
REFERENCES [dbo].[Pricing] ([Id])
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD FOREIGN KEY([ToAreaId])
REFERENCES [dbo].[Area] ([Id])
GO
ALTER TABLE [dbo].[Wallet]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
USE [master]
GO
ALTER DATABASE [Share_Taxi] SET  READ_WRITE 
GO
