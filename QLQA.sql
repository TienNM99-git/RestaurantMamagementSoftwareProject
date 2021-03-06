USE [master]
GO
/****** Object:  Database [QuanLyQuanAn]    Script Date: 1/1/2020 8:21:06 PM ******/
CREATE DATABASE [QuanLyQuanAn]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QuanLyQuanAn', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQL2017\MSSQL\DATA\QuanLyQuanAn.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QuanLyQuanAn_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQL2017\MSSQL\DATA\QuanLyQuanAn_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QuanLyQuanAn].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ARITHABORT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QuanLyQuanAn] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QuanLyQuanAn] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET  ENABLE_BROKER 
GO
ALTER DATABASE [QuanLyQuanAn] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QuanLyQuanAn] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET RECOVERY FULL 
GO
ALTER DATABASE [QuanLyQuanAn] SET  MULTI_USER 
GO
ALTER DATABASE [QuanLyQuanAn] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QuanLyQuanAn] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QuanLyQuanAn] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QuanLyQuanAn] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'QuanLyQuanAn', N'ON'
GO
USE [QuanLyQuanAn]
GO
/****** Object:  User [tiennm1999]    Script Date: 1/1/2020 8:21:06 PM ******/
CREATE USER [tiennm1999] FOR LOGIN [tiennm1999] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[NormalizeString]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[NormalizeString]
(
@strInput nvarchar(4000)
)
RETURNS nvarchar(4000)
AS
BEGIN
SET @strInput = RTRIM(LTRIM(LOWER(@strInput)));
IF @strInput IS NULL
BEGIN
RETURN @strInput;
END;
IF @strInput = ''
BEGIN
RETURN @strInput;
END;
DECLARE @text nvarchar(50), @i int;
SET @text = '-''`~!@#$%^&*()?><:|}{,./\"''='';–';
SELECT @i = PATINDEX('%['+@text+']%', @strInput);
WHILE @i > 0
BEGIN
SET @strInput = replace(@strInput, SUBSTRING(@strInput, @i, 1), '');
SET @i = PATINDEX('%['+@text+']%', @strInput);
END;
SET @strInput = replace(@strInput, ' ', ' ');

DECLARE @RT nvarchar(4000);
DECLARE @SIGN_CHARS nchar(136);
DECLARE @UNSIGN_CHARS nchar(136);
SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế
ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý'+NCHAR(272)+NCHAR(208);
SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee
iiiiiooooooooooooooouuuuuuuuuuyyyyy';
DECLARE @COUNTER int;
DECLARE @COUNTER1 int;
SET @COUNTER = 1;
WHILE(@COUNTER <= LEN(@strInput))
BEGIN
SET @COUNTER1 = 1;
WHILE(@COUNTER1 <= LEN(@SIGN_CHARS) + 1)
BEGIN
IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1, 1)) = UNICODE(SUBSTRING(@strInput, @COUNTER, 1))
BEGIN
IF @COUNTER = 1
BEGIN
SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1, 1) + SUBSTRING(@strInput, @COUNTER+1, LEN(@strInput)-1);
END;
ELSE
BEGIN
SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) + SUBSTRING(@UNSIGN_CHARS, @COUNTER1, 1) + SUBSTRING(@strInput, @COUNTER+1, LEN(@strInput)-@COUNTER);
END;
BREAK;
END;
SET @COUNTER1 = @COUNTER1 + 1;
END;
SET @COUNTER = @COUNTER + 1;
END;
SET @strInput = replace(@strInput, ' ', '-');
RETURN LOWER(@strInput);
END;
GO
/****** Object:  Table [dbo].[Account]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[UserName] [nvarchar](100) NOT NULL,
	[DisplayName] [nvarchar](100) NOT NULL,
	[Pass] [nvarchar](1000) NOT NULL,
	[Typ] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[DateCheckIn] [date] NOT NULL,
	[DateCheckOut] [date] NULL,
	[idTable] [int] NOT NULL,
	[stat] [int] NOT NULL,
	[totalPrice] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillInfo]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idBill] [int] NOT NULL,
	[idFood] [int] NOT NULL,
	[count] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CusTable]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CusTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[stat] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Food]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[idCategory] [int] NOT NULL,
	[Price] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FoodCategory]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FoodCategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [NonClusteredIndex-20191227-223427]    Script Date: 1/1/2020 8:21:06 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20191227-223427] ON [dbo].[Food]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__Display]  DEFAULT (N'User') FOR [DisplayName]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__Pass]  DEFAULT ((0)) FOR [Pass]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__Typ]  DEFAULT ((0)) FOR [Typ]
GO
ALTER TABLE [dbo].[Bill] ADD  CONSTRAINT [DF__Bill__DateCheckIn]  DEFAULT (getdate()) FOR [DateCheckIn]
GO
ALTER TABLE [dbo].[Bill] ADD  CONSTRAINT [DF__Bill__stat]  DEFAULT ((0)) FOR [stat]
GO
ALTER TABLE [dbo].[BillInfo] ADD  CONSTRAINT [DF__BillInfo__count]  DEFAULT ((0)) FOR [count]
GO
ALTER TABLE [dbo].[CusTable] ADD  DEFAULT (N'No name') FOR [name]
GO
ALTER TABLE [dbo].[CusTable] ADD  CONSTRAINT [DF__CusTable__stat]  DEFAULT (N'Empty') FOR [stat]
GO
ALTER TABLE [dbo].[Food] ADD  CONSTRAINT [DF__Food__name]  DEFAULT (N'No name') FOR [name]
GO
ALTER TABLE [dbo].[Food] ADD  CONSTRAINT [DF__Food__Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[FoodCategory] ADD  DEFAULT (N'No name') FOR [name]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD FOREIGN KEY([idTable])
REFERENCES [dbo].[CusTable] ([id])
GO
ALTER TABLE [dbo].[BillInfo]  WITH CHECK ADD FOREIGN KEY([idBill])
REFERENCES [dbo].[Bill] ([id])
GO
ALTER TABLE [dbo].[BillInfo]  WITH CHECK ADD FOREIGN KEY([idFood])
REFERENCES [dbo].[Food] ([id])
GO
ALTER TABLE [dbo].[Food]  WITH CHECK ADD FOREIGN KEY([idCategory])
REFERENCES [dbo].[FoodCategory] ([id])
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [Cons_Status] CHECK  (([stat]=(1) OR [stat]=(0)))
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [Cons_Status]
GO
/****** Object:  StoredProcedure [dbo].[UPS_GetTableList]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[UPS_GetTableList]
AS SELECT * FROM dbo.CusTable
GO
/****** Object:  StoredProcedure [dbo].[UPS_Login]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Login
CREATE PROC [dbo].[UPS_Login]
@userName NVARCHAR(100), @passWord NVARCHAR(1000)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND Pass = @passWord
END
GO
/****** Object:  StoredProcedure [dbo].[UPS_Menu]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[UPS_Menu]
@idTable INT
AS
BEGIN
	SELECT f.name, bi.count, f.Price, f.Price * bi.count AS Total 
	FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f 
	WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.stat = 0 AND b.idTable = @idTable
END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetListBillByDate]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   proc [dbo].[USP_GetListBillByDate]
@checkIn date, @checkOut date
as begin
 select t.name as [Table Name],b.totalPrice as [Total Price],DateCheckIn as [Check In Time],DateCheckOut as [Check Out Time]
 from dbo.Bill as b, dbo.CusTable as t
 where DateCheckIn>=@checkIn and DateCheckOut <=@checkOut and B.stat=1
 and t.id=b.idTable
end
GO
/****** Object:  StoredProcedure [dbo].[USP_InserBill]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[USP_InserBill]
@idTable INT
AS
BEGIN
	INSERT dbo.Bill
	        ( DateCheckIn ,
	          DateCheckOut ,
	          idTable ,
	          stat
	        )
	VALUES  ( GETDATE() , -- DateCheckIn - date
	          NULL , -- DateCheckOut - date
	          @idTable , -- idTable - int
	          0  -- stat - int
	        )
END
GO
/****** Object:  StoredProcedure [dbo].[USP_InserBillInfo]    Script Date: 1/1/2020 8:21:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_InserBillInfo]
@idBill INT, @idFood INT, @count INT
AS
BEGIN
	DECLARE @isExistBillInfo  INT;
	DECLARE @foodCount INT = 1;
	
	SELECT @isExistBillInfo = id, @foodCount = b.count
	FROM dbo.BillInfo  as b
	WHERE idBill = @idBill AND idFood = @idFood

	IF(@isExistBillInfo > 0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF(@newCount > 0)
			UPDATE dbo.BillInfo SET count = @newCount WHERE idFood = @idFood AND idBill = @idBill
		ELSE
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
	END
    ELSE
	BEGIN
		INSERT dbo.BillInfo
			( idBill, idFood, count )
		VALUES  ( @idBill, -- idBill - int
			@idFood, -- idFood - int
			@count  -- count - int
			)
	END
END
GO
USE [master]
GO
ALTER DATABASE [QuanLyQuanAn] SET  READ_WRITE 
GO
