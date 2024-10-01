USE [master]
GO
/****** Object:  Database [Share_Taxi]    Script Date: 01/10/2024 2:13:21 CH ******/
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
/****** Object:  Table [dbo].[Area]    Script Date: 01/10/2024 2:13:21 CH ******/
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
/****** Object:  Table [dbo].[Booking]    Script Date: 01/10/2024 2:13:21 CH ******/
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
/****** Object:  Table [dbo].[CarTrip]    Script Date: 01/10/2024 2:13:21 CH ******/
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
/****** Object:  Table [dbo].[Deposit]    Script Date: 01/10/2024 2:13:21 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deposit](
	[Id] [int] NOT NULL,
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
/****** Object:  Table [dbo].[Location]    Script Date: 01/10/2024 2:13:21 CH ******/
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
/****** Object:  Table [dbo].[Transaction]    Script Date: 01/10/2024 2:13:21 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [int] NOT NULL,
	[DepositId] [int] NULL,
	[WalletId] [int] NULL,
	[Amount] [decimal](10, 2) NULL,
	[TransactionType] [nvarchar](255) NULL,
	[ReferenceId] [nvarchar](255) NULL,
	[Status] [int] NULL,
	[CreatedAt] [datetime] NULL,
 CONSTRAINT [PK__Transact__3214EC0753E4BED1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trip]    Script Date: 01/10/2024 2:13:21 CH ******/
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
	[TripTypeId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TripeTypePricing]    Script Date: 01/10/2024 2:13:21 CH ******/
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
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TripType]    Script Date: 01/10/2024 2:13:21 CH ******/
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
	[Status] [int] NULL,
	[BasicePrice] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK__TripType__3214EC070242C8F9] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 01/10/2024 2:13:21 CH ******/
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
/****** Object:  Table [dbo].[Wallet]    Script Date: 01/10/2024 2:13:21 CH ******/
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
REFERENCES [dbo].[Location] ([ID])
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD FOREIGN KEY([pickUpLocationId])
REFERENCES [dbo].[Location] ([ID])
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
