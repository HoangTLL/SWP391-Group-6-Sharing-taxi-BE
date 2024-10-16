USE [master]
GO
/****** Object:  Database [Share_Taxi]    Script Date: 16/10/2024 3:14:22 CH ******/
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
/****** Object:  Table [dbo].[Area]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Area](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[TripId] [int] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CarTrip]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarTrip](
	[Id] [int] IDENTITY(1,1) NOT NULL,
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
/****** Object:  Table [dbo].[Deposit]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deposit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WalletId] [int] NULL,
	[UserId] [int] NOT NULL,
	[Amount] [decimal](10, 2) NULL,
	[DepositMethod] [nvarchar](255) NULL,
	[DepositDate] [datetime] NULL,
	[Status] [int] NULL,
	[TransactionId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Lat] [decimal](10, 8) NULL,
	[Lon] [decimal](11, 8) NULL,
	[AreaId] [int] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DepositId] [int] NULL,
	[WalletId] [int] NULL,
	[Amount] [decimal](10, 2) NULL,
	[TransactionType] [nvarchar](255) NULL,
	[ReferenceId] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK__Transact__3214EC0753E4BED1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trip]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trip](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[pickUpLocationId] [int] NULL,
	[dropOffLocationId] [int] NULL,
	[ToAreaId] [int] NULL,
	[MaxPerson] [int] NULL,
	[MinPerson] [int] NULL,
	[UnitPrice] [decimal](10, 2) NULL,
	[BookingDate] [date] NULL,
	[HourInDay] [time](7) NULL,
	[TripTypeId] [int] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK__Trip__3214EC0792BD0D84] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TripeTypePricing]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TripeTypePricing](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[TripType] [int] NOT NULL,
	[MinPerson] [int] NOT NULL,
	[MaxPerson] [int] NOT NULL,
	[PricePerPerson] [decimal](10, 2) NOT NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TripType]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TripType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FromAreaId] [int] NOT NULL,
	[ToAreaId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[BasicePrice] [decimal](10, 2) NOT NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK__TripType__3214EC070242C8F9] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[DateOfBirth] [date] NULL,
	[CreatedAt] [datetime] NULL,
	[Role] [nvarchar](255) NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 16/10/2024 3:14:22 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Balance] [decimal](10, 2) NULL,
	[CurrencyCode] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Area] ON 

INSERT [dbo].[Area] ([Id], [Name], [Description], [Status]) VALUES (1, N'VinHome', N'Khu dan cu Vinhome thuoc tap doan vin group gom có 2 phan khu chinh (Rainbow va Origami) ', 1)
INSERT [dbo].[Area] ([Id], [Name], [Description], [Status]) VALUES (2, N'Nha Van Hoa Sinh Viên', N'Thuoc lang dai hoc la khu hoc tap danh cho sinh vien ki 5 nghanh IT cua dai hoc FPT', 1)
INSERT [dbo].[Area] ([Id], [Name], [Description], [Status]) VALUES (3, N'Ðai hoc FPT', N'La truong dai hoc thuoc khu cong nghe cao thanh pho Thu Ðuc', 1)
SET IDENTITY_INSERT [dbo].[Area] OFF
GO
SET IDENTITY_INSERT [dbo].[Booking] ON 

INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (1, 10, 2, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (4, 11, 2, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (5, 10, 3, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (6, 11, 3, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (7, 1, 3, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (8, 1, 4, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (9, 11, 4, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (10, 10, 4, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (11, 1, 5, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (12, 11, 5, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (13, 10, 5, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (14, 1, 6, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (15, 11, 6, 1)
INSERT [dbo].[Booking] ([Id], [UserId], [TripId], [Status]) VALUES (16, 10, 6, 1)
SET IDENTITY_INSERT [dbo].[Booking] OFF
GO
SET IDENTITY_INSERT [dbo].[Deposit] ON 

INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (3, 1, 10, CAST(123.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-10T18:53:13.033' AS DateTime), 1, NULL)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (4, 1, 10, CAST(100.00 AS Decimal(10, 2)), N'momo', CAST(N'2024-10-10T18:55:40.150' AS DateTime), 1, 4)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (5, 1, 10, CAST(2.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-11T13:31:42.173' AS DateTime), 1, 5)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (6, 2, 11, CAST(100.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-14T20:50:54.270' AS DateTime), 1, 8)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (7, 2, 11, CAST(100.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-14T21:08:20.733' AS DateTime), 1, 10)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (8, 1, 10, CAST(175.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-14T21:09:47.637' AS DateTime), 1, 11)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (9, 1, 10, CAST(800.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-14T21:12:12.150' AS DateTime), 1, 12)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (10, 2, 11, CAST(800.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-14T21:12:17.587' AS DateTime), 1, 13)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (11, 2, 11, CAST(800.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-14T21:12:21.370' AS DateTime), 1, 14)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (12, 1, 10, CAST(1000.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-14T21:29:19.830' AS DateTime), 1, 21)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (13, 2, 11, CAST(1000.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-14T21:29:22.917' AS DateTime), 1, 22)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (14, 1, 10, CAST(1000.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-14T21:37:54.943' AS DateTime), 1, 29)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (15, 3, 1, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-15T18:57:05.767' AS DateTime), 1, 349)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (16, 3, 1, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-15T18:57:05.847' AS DateTime), 1, 350)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (17, 2, 11, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-15T18:57:05.873' AS DateTime), 1, 351)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (18, 1, 10, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-15T18:57:05.900' AS DateTime), 1, 352)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (19, 3, 1, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-15T18:57:05.923' AS DateTime), 1, 353)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (20, 2, 11, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-15T18:57:05.947' AS DateTime), 1, 354)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (21, 1, 10, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-15T18:57:05.973' AS DateTime), 1, 355)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (22, 3, 1, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-16T01:11:04.540' AS DateTime), 1, 362)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (23, 2, 11, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-16T01:11:04.737' AS DateTime), 1, 363)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (24, 1, 10, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-16T01:11:04.813' AS DateTime), 1, 364)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (25, 3, 1, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-16T01:11:04.907' AS DateTime), 1, 365)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (26, 2, 11, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-16T01:11:04.970' AS DateTime), 1, 366)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (27, 1, 10, CAST(37.50 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-16T01:11:05.017' AS DateTime), 1, 367)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (28, 3, 1, CAST(25.00 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-16T03:09:23.717' AS DateTime), 1, 371)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (29, 2, 11, CAST(25.00 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-16T03:09:39.233' AS DateTime), 1, 372)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (30, 1, 10, CAST(25.00 AS Decimal(10, 2)), N'Refund', CAST(N'2024-10-16T03:09:46.840' AS DateTime), 1, 373)
INSERT [dbo].[Deposit] ([Id], [WalletId], [UserId], [Amount], [DepositMethod], [DepositDate], [Status], [TransactionId]) VALUES (31, 1, 10, CAST(100.00 AS Decimal(10, 2)), N'vnpay', CAST(N'2024-10-16T14:57:02.493' AS DateTime), 1, 374)
SET IDENTITY_INSERT [dbo].[Deposit] OFF
GO
SET IDENTITY_INSERT [dbo].[Location] ON 

INSERT [dbo].[Location] ([Id], [Name], [Lat], [Lon], [AreaId], [Status]) VALUES (1, N'Toa S5.01 - Vinhome', CAST(21.03065300 AS Decimal(10, 8)), CAST(105.84713000 AS Decimal(11, 8)), 1, NULL)
INSERT [dbo].[Location] ([Id], [Name], [Lat], [Lon], [AreaId], [Status]) VALUES (2, N'Toa S2.02 - Vinhome', CAST(21.03065200 AS Decimal(10, 8)), CAST(105.84823000 AS Decimal(11, 8)), 1, NULL)
INSERT [dbo].[Location] ([Id], [Name], [Lat], [Lon], [AreaId], [Status]) VALUES (3, N'Toa S6.01 - Vinhome', CAST(24.40065200 AS Decimal(10, 8)), CAST(135.82823000 AS Decimal(11, 8)), 1, NULL)
INSERT [dbo].[Location] ([Id], [Name], [Lat], [Lon], [AreaId], [Status]) VALUES (4, N'Toa S8.01 - Vinhome', CAST(30.40067000 AS Decimal(10, 8)), CAST(200.88923000 AS Decimal(11, 8)), 1, NULL)
INSERT [dbo].[Location] ([Id], [Name], [Lat], [Lon], [AreaId], [Status]) VALUES (5, N'Toa S10.01 - Vinhome', CAST(41.39165200 AS Decimal(10, 8)), CAST(245.64523000 AS Decimal(11, 8)), 1, NULL)
INSERT [dbo].[Location] ([Id], [Name], [Lat], [Lon], [AreaId], [Status]) VALUES (6, N'Cong Chinh - ÐH FPT', CAST(21.01203300 AS Decimal(10, 8)), CAST(105.52528400 AS Decimal(11, 8)), 3, NULL)
INSERT [dbo].[Location] ([Id], [Name], [Lat], [Lon], [AreaId], [Status]) VALUES (7, N'Cong Phu - ÐH FPT', CAST(24.40065200 AS Decimal(10, 8)), CAST(135.82823000 AS Decimal(11, 8)), 3, NULL)
INSERT [dbo].[Location] ([Id], [Name], [Lat], [Lon], [AreaId], [Status]) VALUES (8, N'Cong vao nha van hoa - NHVSV', CAST(10.76262200 AS Decimal(10, 8)), CAST(106.66017200 AS Decimal(11, 8)), 2, NULL)
INSERT [dbo].[Location] ([Id], [Name], [Lat], [Lon], [AreaId], [Status]) VALUES (9, N'Cong nha van hoa - NHVSV', CAST(10.84562200 AS Decimal(10, 8)), CAST(135.73452000 AS Decimal(11, 8)), 2, NULL)
SET IDENTITY_INSERT [dbo].[Location] OFF
GO
SET IDENTITY_INSERT [dbo].[Transaction] ON 

INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (204, NULL, 1, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.263' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (205, NULL, 2, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.273' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (206, NULL, 3, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.290' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (207, NULL, 1, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.303' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (208, NULL, 2, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.317' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (209, NULL, 3, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.323' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (210, NULL, 1, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.363' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (211, NULL, 2, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.383' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (212, NULL, 3, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.400' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (213, NULL, 1, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.417' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (214, NULL, 2, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.433' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (215, NULL, 3, CAST(50.46 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T14:49:12.457' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (216, NULL, 1, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_3', CAST(N'2024-10-15T17:41:27.013' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (217, NULL, 2, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_3', CAST(N'2024-10-15T17:41:27.120' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (218, NULL, 3, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_3', CAST(N'2024-10-15T17:41:27.140' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (219, NULL, 1, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_3', CAST(N'2024-10-15T17:45:28.403' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (220, NULL, 2, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_3', CAST(N'2024-10-15T17:45:28.493' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (221, NULL, 3, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_3', CAST(N'2024-10-15T17:45:28.507' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (222, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:05.870' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (223, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.177' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (224, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.197' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (225, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.213' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (226, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.233' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (227, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.250' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (228, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.267' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (229, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.283' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (230, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.297' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (231, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.313' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (232, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.330' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (233, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.340' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (234, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.350' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (235, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.357' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (236, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.373' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (237, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.387' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (238, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.397' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (239, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.420' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (240, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.430' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (241, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.440' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (242, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.450' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (243, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.460' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (244, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.470' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (245, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.483' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (246, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.493' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (247, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.507' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (248, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.520' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (249, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.533' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (250, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.543' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (251, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.557' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (252, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.587' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (253, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.600' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (254, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.610' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (255, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.620' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (256, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.633' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (257, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.640' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (258, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.653' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (259, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.663' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (260, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.673' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (261, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.683' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (262, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.690' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (263, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.700' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (264, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.710' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (265, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.720' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (266, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.727' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (267, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.737' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (268, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.747' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (269, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.757' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (270, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.767' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (271, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.773' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (272, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.787' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (273, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.793' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (274, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.803' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (275, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.813' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (276, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.823' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (277, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.833' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (278, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.847' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (279, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.853' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (280, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.867' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (281, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:27:06.873' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (282, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.377' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (283, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.643' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (284, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.663' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (285, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.680' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (286, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.690' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (287, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.697' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (288, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.703' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (289, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.713' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (290, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.720' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (291, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.727' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (292, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.733' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (293, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.817' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (294, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.843' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (295, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.867' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (296, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.910' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (297, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.933' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (298, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.950' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (299, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.960' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (300, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.970' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (301, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.983' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (302, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:45.993' AS DateTime), 1)
GO
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (303, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.000' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (304, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.013' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (305, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.027' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (306, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.047' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (307, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.060' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (308, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.067' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (309, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.080' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (310, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.093' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (311, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.100' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (312, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.117' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (313, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.130' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (314, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.147' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (315, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.173' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (316, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.190' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (317, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.203' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (318, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.213' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (319, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.223' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (320, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.237' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (321, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.250' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (322, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.267' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (323, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.283' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (324, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.297' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (325, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.310' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (326, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.320' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (327, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.330' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (328, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.343' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (329, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.353' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (330, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.363' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (331, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.373' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (332, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.383' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (333, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.400' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (334, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.413' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (335, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.427' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (336, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.440' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (337, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.450' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (338, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.463' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (339, NULL, 1, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.480' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (340, NULL, 2, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.493' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (341, NULL, 3, CAST(82.92 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_3', CAST(N'2024-10-15T18:32:46.507' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (342, NULL, 3, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_4', CAST(N'2024-10-15T18:51:13.740' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (343, NULL, 3, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_4', CAST(N'2024-10-15T18:52:12.920' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (344, NULL, 2, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_4', CAST(N'2024-10-15T18:52:12.937' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (345, NULL, 1, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_4', CAST(N'2024-10-15T18:52:12.953' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (346, NULL, 3, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_4', CAST(N'2024-10-15T18:53:43.853' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (347, NULL, 2, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_4', CAST(N'2024-10-15T18:53:43.920' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (348, NULL, 1, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_4', CAST(N'2024-10-15T18:53:43.933' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (349, 15, 3, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_4', CAST(N'2024-10-15T18:57:05.527' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (350, 16, 3, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_4', CAST(N'2024-10-15T18:57:05.837' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (351, 17, 2, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_4', CAST(N'2024-10-15T18:57:05.860' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (352, 18, 1, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_4', CAST(N'2024-10-15T18:57:05.890' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (353, 19, 3, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_4', CAST(N'2024-10-15T18:57:05.913' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (354, 20, 2, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_4', CAST(N'2024-10-15T18:57:05.937' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (355, 21, 1, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_4', CAST(N'2024-10-15T18:57:05.960' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (356, NULL, 3, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_5', CAST(N'2024-10-16T01:09:58.873' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (357, NULL, 2, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_5', CAST(N'2024-10-16T01:09:59.170' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (358, NULL, 1, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_5', CAST(N'2024-10-16T01:09:59.217' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (359, NULL, 3, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_5', CAST(N'2024-10-16T01:11:00.117' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (360, NULL, 2, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_5', CAST(N'2024-10-16T01:11:00.390' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (361, NULL, 1, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_5', CAST(N'2024-10-16T01:11:00.447' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (362, 22, 3, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_5', CAST(N'2024-10-16T01:11:04.457' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (363, 23, 2, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_5', CAST(N'2024-10-16T01:11:04.707' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (364, 24, 1, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_5', CAST(N'2024-10-16T01:11:04.780' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (365, 25, 3, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_5', CAST(N'2024-10-16T01:11:04.860' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (366, 26, 2, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_5', CAST(N'2024-10-16T01:11:04.940' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (367, 27, 1, CAST(37.50 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_5', CAST(N'2024-10-16T01:11:04.990' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (368, NULL, 3, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_6', CAST(N'2024-10-16T02:46:07.763' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (369, NULL, 2, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_6', CAST(N'2024-10-16T02:46:08.087' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (370, NULL, 1, CAST(75.00 AS Decimal(10, 2)), N'Payment', N'Trip_6', CAST(N'2024-10-16T02:46:08.150' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (371, 28, 3, CAST(25.00 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_6', CAST(N'2024-10-16T03:09:18.077' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (372, 29, 2, CAST(25.00 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_6', CAST(N'2024-10-16T03:09:38.020' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (373, 30, 1, CAST(25.00 AS Decimal(10, 2)), N'Refund', N'Refund_Trip_6', CAST(N'2024-10-16T03:09:42.020' AS DateTime), 1)
INSERT [dbo].[Transaction] ([Id], [DepositId], [WalletId], [Amount], [TransactionType], [ReferenceId], [CreatedAt], [Status]) VALUES (374, 31, 1, CAST(100.00 AS Decimal(10, 2)), N'Deposit', N'Deposit_1_638646874232423435', CAST(N'2024-10-16T14:57:03.243' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Transaction] OFF
GO
SET IDENTITY_INSERT [dbo].[Trip] ON 

INSERT [dbo].[Trip] ([Id], [pickUpLocationId], [dropOffLocationId], [ToAreaId], [MaxPerson], [MinPerson], [UnitPrice], [BookingDate], [HourInDay], [TripTypeId], [Status]) VALUES (1, 1, 2, 3, 4, 1, CAST(150.00 AS Decimal(10, 2)), CAST(N'2024-10-11' AS Date), CAST(N'14:00:00' AS Time), 2, 1)
INSERT [dbo].[Trip] ([Id], [pickUpLocationId], [dropOffLocationId], [ToAreaId], [MaxPerson], [MinPerson], [UnitPrice], [BookingDate], [HourInDay], [TripTypeId], [Status]) VALUES (2, 1, 8, 2, 4, 2, CAST(75.00 AS Decimal(10, 2)), CAST(N'2024-10-14' AS Date), CAST(N'10:00:00' AS Time), 1, 1)
INSERT [dbo].[Trip] ([Id], [pickUpLocationId], [dropOffLocationId], [ToAreaId], [MaxPerson], [MinPerson], [UnitPrice], [BookingDate], [HourInDay], [TripTypeId], [Status]) VALUES (3, 1, 8, 2, 4, 2, CAST(75.00 AS Decimal(10, 2)), NULL, CAST(N'14:00:00' AS Time), 1, 1)
INSERT [dbo].[Trip] ([Id], [pickUpLocationId], [dropOffLocationId], [ToAreaId], [MaxPerson], [MinPerson], [UnitPrice], [BookingDate], [HourInDay], [TripTypeId], [Status]) VALUES (4, 1, 8, 2, 4, 2, CAST(75.00 AS Decimal(10, 2)), NULL, CAST(N'14:00:00' AS Time), 1, 3)
INSERT [dbo].[Trip] ([Id], [pickUpLocationId], [dropOffLocationId], [ToAreaId], [MaxPerson], [MinPerson], [UnitPrice], [BookingDate], [HourInDay], [TripTypeId], [Status]) VALUES (5, 1, 8, 2, 4, 2, CAST(75.00 AS Decimal(10, 2)), NULL, CAST(N'14:00:00' AS Time), 1, 3)
INSERT [dbo].[Trip] ([Id], [pickUpLocationId], [dropOffLocationId], [ToAreaId], [MaxPerson], [MinPerson], [UnitPrice], [BookingDate], [HourInDay], [TripTypeId], [Status]) VALUES (6, 1, 8, 2, 4, 2, CAST(150.00 AS Decimal(10, 2)), NULL, NULL, 1, 3)
SET IDENTITY_INSERT [dbo].[Trip] OFF
GO
SET IDENTITY_INSERT [dbo].[TripeTypePricing] ON 

INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (1, N'Vin to NVH', 1, 1, 1, CAST(150.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (2, N'Vin to NVH', 1, 1, 2, CAST(150.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (3, N'Vin to NVH', 1, 1, 3, CAST(150.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (4, N'Vin to NVH', 1, 1, 4, CAST(150.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (5, N'Vin to NVH', 1, 2, 2, CAST(75.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (6, N'Vin to NVH', 1, 2, 3, CAST(75.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (7, N'Vin to NVH', 1, 2, 4, CAST(75.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (8, N'Vin to NVH', 1, 3, 3, CAST(50.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (9, N'Vin to NVH', 1, 3, 4, CAST(50.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (10, N'Vin to NVH', 1, 4, 4, CAST(37.50 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (11, N'Vin to FPT', 2, 1, 1, CAST(100.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (12, N'Vin to FPT', 2, 1, 2, CAST(100.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (13, N'Vin to FPT', 2, 1, 3, CAST(100.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (14, N'Vin to FPT', 2, 1, 4, CAST(100.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (15, N'Vin to FPT', 2, 2, 2, CAST(50.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (16, N'Vin to FPT', 2, 2, 3, CAST(50.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (17, N'Vin to FPT', 2, 2, 4, CAST(50.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (18, N'Vin to FPT', 2, 3, 3, CAST(33.40 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (19, N'Vin to FPT', 2, 3, 4, CAST(33.40 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (20, N'Vin to FPT', 2, 4, 4, CAST(25.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (21, N'NVH to Vin', 3, 1, 1, CAST(150.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (22, N'NVH to Vin', 3, 1, 2, CAST(150.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (23, N'NVH to Vin', 3, 1, 3, CAST(150.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (24, N'NVH to Vin', 3, 1, 4, CAST(150.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (25, N'NVH to Vin', 3, 2, 2, CAST(75.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (26, N'NVH to Vin', 3, 2, 3, CAST(75.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (27, N'NVH to Vin', 3, 2, 4, CAST(75.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (28, N'NVH to Vin', 3, 3, 3, CAST(50.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (29, N'NVH to Vin', 3, 3, 4, CAST(50.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (30, N'NVH to Vin', 3, 4, 4, CAST(37.50 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (31, N'FPT to Vin', 4, 1, 1, CAST(100.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (32, N'FPT to Vin', 4, 1, 2, CAST(100.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (33, N'FPT to Vin', 4, 1, 3, CAST(100.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (34, N'FPT to Vin', 4, 1, 4, CAST(100.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (35, N'FPT to Vin', 4, 2, 2, CAST(50.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (36, N'FPT to Vin', 4, 2, 3, CAST(50.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (37, N'FPT to Vin', 4, 2, 4, CAST(50.00 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (38, N'FPT to Vin', 4, 3, 3, CAST(33.40 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (39, N'FPT to Vin', 4, 3, 4, CAST(33.40 AS Decimal(10, 2)), NULL)
INSERT [dbo].[TripeTypePricing] ([Id], [Name], [TripType], [MinPerson], [MaxPerson], [PricePerPerson], [Status]) VALUES (40, N'FPT to Vin', 4, 4, 4, CAST(25.00 AS Decimal(10, 2)), NULL)
SET IDENTITY_INSERT [dbo].[TripeTypePricing] OFF
GO
SET IDENTITY_INSERT [dbo].[TripType] ON 

INSERT [dbo].[TripType] ([Id], [FromAreaId], [ToAreaId], [Name], [Description], [BasicePrice], [Status]) VALUES (1, 1, 2, N'Vin to NVH', N'Tu VinHome den Nha Van Hoa Sinh Vien', CAST(150.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[TripType] ([Id], [FromAreaId], [ToAreaId], [Name], [Description], [BasicePrice], [Status]) VALUES (2, 1, 3, N'Vin to FPT', N'Tu VinHome den Ðai hoc FPT', CAST(100.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[TripType] ([Id], [FromAreaId], [ToAreaId], [Name], [Description], [BasicePrice], [Status]) VALUES (3, 2, 1, N'NVH to Vin', N'Tu Nha Van Hoa Sinh Vien den Vinhome', CAST(150.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[TripType] ([Id], [FromAreaId], [ToAreaId], [Name], [Description], [BasicePrice], [Status]) VALUES (4, 3, 1, N'FPT to Vin', N'Tu Dai Hoc FPT den Vinhome', CAST(100.00 AS Decimal(10, 2)), 1)
SET IDENTITY_INSERT [dbo].[TripType] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (1, N'Nguyen Van Phuc', N'nguyenvanphuc@gmail.com', N'0868355460', N'Phucnv38', CAST(N'2004-03-08' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'admin', NULL)
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (2, N'Tran Thi Dung', N'tranthidung@gmail.com', N'0912345679', N'Dungtt239', CAST(N'2004-08-25' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'staff', NULL)
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (3, N'Le Van Cuong', N'levancuong@gmail.com', N'0912345680', N'Cuonglv220', CAST(N'2002-02-20' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'user', NULL)
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (4, N'Pham Thi Hoa', N'phamthihoa@gmail.com', N'0912345681', N'Hoapt1214', CAST(N'2003-12-14' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'user', NULL)
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (5, N'Hoang Minh Tri', N'hoangminhtri@gmail.com', N'0912345682', N'Trihm79', CAST(N'2004-07-09' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'user', NULL)
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (6, N'Vu Hong Phuc', N'vuhongphuc@gmail.com', N'0912345683', N'Phucvh330', CAST(N'2003-03-30' AS Date), CAST(N'1894-07-01T00:00:00.000' AS DateTime), N'user', NULL)
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (7, NULL, N'phucnguye5640@gmail.com', NULL, N'Dramon12345', NULL, CAST(N'2024-10-10T15:16:32.017' AS DateTime), N'user', NULL)
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (8, N'daun', N'hoang45@gmail.com', N'034902353', N'hoang345', CAST(N'2004-10-03' AS Date), CAST(N'2024-10-10T17:58:31.617' AS DateTime), N'user', NULL)
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (9, N'strsding', N'string@gmail.com', N'0239529073', N'24324235', CAST(N'2024-10-10' AS Date), CAST(N'2024-10-10T18:05:00.730' AS DateTime), N'user', NULL)
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (10, N'string', N'sstring@gmail.com', N'091247012375', N'dgfkjsgbf34', CAST(N'2024-10-10' AS Date), CAST(N'2024-10-10T18:06:51.423' AS DateTime), N'user', NULL)
INSERT [dbo].[User] ([Id], [Name], [Email], [PhoneNumber], [Password], [DateOfBirth], [CreatedAt], [Role], [Status]) VALUES (11, N'Nguyen Thanh Dat', N'Datnguyen9240@gmail.com', N'01247095124', N'Datnt03', CAST(N'2004-03-03' AS Date), CAST(N'2024-10-11T13:36:49.873' AS DateTime), N'user', NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[Wallet] ON 

INSERT [dbo].[Wallet] ([Id], [UserId], [Balance], [CurrencyCode], [CreatedAt], [UpdatedAt], [Status]) VALUES (1, 10, CAST(7349.71 AS Decimal(10, 2)), N'USD', CAST(N'2024-10-10T18:06:51.423' AS DateTime), NULL, 1)
INSERT [dbo].[Wallet] ([Id], [UserId], [Balance], [CurrencyCode], [CreatedAt], [UpdatedAt], [Status]) VALUES (2, 11, CAST(6465.49 AS Decimal(10, 2)), N'VND', CAST(N'2024-10-11T13:36:49.873' AS DateTime), NULL, 1)
INSERT [dbo].[Wallet] ([Id], [UserId], [Balance], [CurrencyCode], [CreatedAt], [UpdatedAt], [Status]) VALUES (3, 1, CAST(101247.32 AS Decimal(10, 2)), N'VND', CAST(N'2024-10-11T13:36:49.873' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[Wallet] OFF
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK__Booking__TripId__37A5467C] FOREIGN KEY([TripId])
REFERENCES [dbo].[Trip] ([Id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK__Booking__TripId__37A5467C]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[CarTrip]  WITH CHECK ADD  CONSTRAINT [FK__CarTrip__TripId__398D8EEE] FOREIGN KEY([TripId])
REFERENCES [dbo].[Trip] ([Id])
GO
ALTER TABLE [dbo].[CarTrip] CHECK CONSTRAINT [FK__CarTrip__TripId__398D8EEE]
GO
ALTER TABLE [dbo].[Deposit]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Deposit]  WITH CHECK ADD FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([Id])
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD FOREIGN KEY([AreaId])
REFERENCES [dbo].[Area] ([Id])
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK__Transacti__Walle__3E52440B] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK__Transacti__Walle__3E52440B]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Deposit] FOREIGN KEY([DepositId])
REFERENCES [dbo].[Deposit] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Deposit]
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD  CONSTRAINT [FK__Trip__dropOffLoc__3F466844] FOREIGN KEY([dropOffLocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Trip] CHECK CONSTRAINT [FK__Trip__dropOffLoc__3F466844]
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD  CONSTRAINT [FK__Trip__pickUpLoca__403A8C7D] FOREIGN KEY([pickUpLocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Trip] CHECK CONSTRAINT [FK__Trip__pickUpLoca__403A8C7D]
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD  CONSTRAINT [FK_Trip_TripType] FOREIGN KEY([TripTypeId])
REFERENCES [dbo].[TripType] ([Id])
GO
ALTER TABLE [dbo].[Trip] CHECK CONSTRAINT [FK_Trip_TripType]
GO
ALTER TABLE [dbo].[TripeTypePricing]  WITH CHECK ADD  CONSTRAINT [FK_TripTypePricing_TripType] FOREIGN KEY([TripType])
REFERENCES [dbo].[TripType] ([Id])
GO
ALTER TABLE [dbo].[TripeTypePricing] CHECK CONSTRAINT [FK_TripTypePricing_TripType]
GO
ALTER TABLE [dbo].[Wallet]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
USE [master]
GO
ALTER DATABASE [Share_Taxi] SET  READ_WRITE 
GO
