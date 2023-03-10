USE [master]
GO
/****** Object:  Database [EmployeeDemo]    Script Date: 2/7/2023 10:02:12 AM ******/
CREATE DATABASE [EmployeeDemo]
 CONTAINMENT = PARTIAL
 ON  PRIMARY 
( NAME = N'EmployeeDemo', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EmployeeDemo.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EmployeeDemo_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\EmployeeDemo_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [EmployeeDemo] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EmployeeDemo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EmployeeDemo] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EmployeeDemo] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EmployeeDemo] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EmployeeDemo] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EmployeeDemo] SET ARITHABORT OFF 
GO
ALTER DATABASE [EmployeeDemo] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EmployeeDemo] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EmployeeDemo] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EmployeeDemo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EmployeeDemo] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EmployeeDemo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EmployeeDemo] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EmployeeDemo] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EmployeeDemo] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EmployeeDemo] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EmployeeDemo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EmployeeDemo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EmployeeDemo] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EmployeeDemo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EmployeeDemo] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EmployeeDemo] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EmployeeDemo] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EmployeeDemo] SET RECOVERY FULL 
GO
ALTER DATABASE [EmployeeDemo] SET  MULTI_USER 
GO
ALTER DATABASE [EmployeeDemo] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EmployeeDemo] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EmployeeDemo] SET DEFAULT_FULLTEXT_LANGUAGE = 1033 
GO
ALTER DATABASE [EmployeeDemo] SET DEFAULT_LANGUAGE = 1033 
GO
ALTER DATABASE [EmployeeDemo] SET NESTED_TRIGGERS = ON 
GO
ALTER DATABASE [EmployeeDemo] SET TRANSFORM_NOISE_WORDS = OFF 
GO
ALTER DATABASE [EmployeeDemo] SET TWO_DIGIT_YEAR_CUTOFF = 2049 
GO
ALTER DATABASE [EmployeeDemo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EmployeeDemo] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EmployeeDemo] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EmployeeDemo] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'EmployeeDemo', N'ON'
GO
ALTER DATABASE [EmployeeDemo] SET QUERY_STORE = OFF
GO
USE [EmployeeDemo]
GO
/****** Object:  User [employee]    Script Date: 2/7/2023 10:02:12 AM ******/
CREATE USER [employee] WITH PASSWORD=N'1F7f9W+grmLi9djBrYwXLiqrZfU0Owhahh3YKTC5MPo=', DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [employee]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [employee]
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 2/7/2023 10:02:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[ContactId] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](200) NOT NULL,
	[ContactType] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 2/7/2023 10:02:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[TaxNumber] [nvarchar](50) NOT NULL,
	[Street] [nvarchar](200) NULL,
	[ZipCode] [nvarchar](100) NULL,
	[CivilParish] [nvarchar](100) NULL,
	[Version] [timestamp] NOT NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DomainEvents]    Script Date: 2/17/2023 12:16:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DomainEvents](
	[DomainEventId] [int] IDENTITY(1,1) NOT NULL,
	[Event] [nvarchar](max) NOT NULL,
	[EventType] [nvarchar](200) NOT NULL,
	[EmployeeId] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [master]
GO
ALTER DATABASE [EmployeeDemo] SET  READ_WRITE 
GO
