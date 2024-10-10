USE [master]
GO
/****** Object:  Database [Share_Taxi]    Script Date: 10/10/2024 12:36:13 CH ******/
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
/****** Object:  Table [dbo].[Area]    Script Date: 10/10/2024 12:36:13 CH ******/
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
/****** Object:  Table [dbo].[Booking]    Script Date: 10/10/2024 12:36:13 CH ******/
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
/****** Object:  Table [dbo].[CarTrip]    Script Date: 10/10/2024 12:36:13 CH ******/
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
/****** Object:  Table [dbo].[Deposit]    Script Date: 10/10/2024 12:36:13 CH ******/
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
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 10/10/2024 12:36:13 CH ******/
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
/****** Object:  Table [dbo].[Transaction]    Script Date: 10/10/2024 12:36:13 CH ******/
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
/****** Object:  Table [dbo].[Trip]    Script Date: 10/10/2024 12:36:13 CH ******/
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
	[BookingDate] [datetime] NULL,
	[HourInDay] [time](7) NULL,
	[PricingId] [int] NULL,
	[TripTypeId] [int] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TripeTypePricing]    Script Date: 10/10/2024 12:36:13 CH ******/
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
/****** Object:  Table [dbo].[TripType]    Script Date: 10/10/2024 12:36:13 CH ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 10/10/2024 12:36:13 CH ******/
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
/****** Object:  Table [dbo].[Wallet]    Script Date: 10/10/2024 12:36:13 CH ******/
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
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD FOREIGN KEY([TripId])
REFERENCES [dbo].[Trip] ([Id])
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[CarTrip]  WITH CHECK ADD FOREIGN KEY([TripId])
REFERENCES [dbo].[Trip] ([Id])
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
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD FOREIGN KEY([dropOffLocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD FOREIGN KEY([pickUpLocationId])
REFERENCES [dbo].[Location] ([Id])
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
