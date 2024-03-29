USE [master]
GO
/****** Object:  Database [EventCalendar]    Script Date: 5/15/2016 12:21:34 PM ******/
CREATE DATABASE [EventCalendar]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EventCalendar', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\EventCalendar.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'EventCalendar_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\EventCalendar_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [EventCalendar] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EventCalendar].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EventCalendar] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EventCalendar] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EventCalendar] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EventCalendar] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EventCalendar] SET ARITHABORT OFF 
GO
ALTER DATABASE [EventCalendar] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EventCalendar] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EventCalendar] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EventCalendar] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EventCalendar] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EventCalendar] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EventCalendar] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EventCalendar] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EventCalendar] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EventCalendar] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EventCalendar] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EventCalendar] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EventCalendar] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EventCalendar] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EventCalendar] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EventCalendar] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EventCalendar] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EventCalendar] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EventCalendar] SET  MULTI_USER 
GO
ALTER DATABASE [EventCalendar] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EventCalendar] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EventCalendar] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EventCalendar] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [EventCalendar] SET DELAYED_DURABILITY = DISABLED 
GO
USE [EventCalendar]
GO
/****** Object:  Table [dbo].[AppUser]    Script Date: 5/15/2016 12:21:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser](
	[AppUserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](20) NOT NULL,
	[LastName] [nvarchar](20) NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](16) NOT NULL,
	[IsAdmin] [bit] NOT NULL,
	[LNumber] [nchar](9) NULL,
	[ConcurrencyId] [int] NOT NULL CONSTRAINT [DF_AppUser_ConcurrencyId]  DEFAULT ((1)),
 CONSTRAINT [PK_AppUser] PRIMARY KEY CLUSTERED 
(
	[AppUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Event]    Script Date: 5/15/2016 12:21:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[EventId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[EventDate] [date] NOT NULL,
	[EventTitle] [nvarchar](20) NOT NULL,
	[EventDescription] [nvarchar](1000) NOT NULL,
	[ConcurrencyId] [int] NOT NULL CONSTRAINT [DF_Event_ConcurrencyId]  DEFAULT ((1)),
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_AppUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[AppUser] ([AppUserID])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_AppUser]
GO
/****** Object:  StoredProcedure [dbo].[usp_EventCreate]    Script Date: 5/15/2016 12:21:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_EventCreate] 
	-- Add the parameters for the stored procedure here
	@EventID int output,
	@UserID int, 
	@EventTitle nvarchar(20), 
	@EventDescription nvarchar(1000), 
	@EventDate date
AS
BEGIN
    -- Insert statements for procedure here
	Insert into Event (UserID, EventTitle, EventDescription, EventDate, ConcurrencyID)
	values (@UserID, @EventTitle, @EventDescription, @EventDate, 1);
	set @EventId = @@IDENTITY;
END


GO
/****** Object:  StoredProcedure [dbo].[usp_EventDelete]    Script Date: 5/15/2016 12:21:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_EventDelete] 
	-- Add the parameters for the stored procedure here
	@EventID int, @ConcurrencyID int
AS
BEGIN
    -- Insert statements for procedure here
	Delete from Event where EventID = @EventID and ConcurrencyID = @ConcurrencyID;
END


GO
/****** Object:  StoredProcedure [dbo].[usp_EventSelect]    Script Date: 5/15/2016 12:21:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_EventSelect]
@EventID int
AS
BEGIN
    select * from Event 
	where EventID = @EventID;
END




GO
/****** Object:  StoredProcedure [dbo].[usp_EventSelectAll]    Script Date: 5/15/2016 12:21:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_EventSelectAll] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
    -- Insert statements for procedure here
	SELECT * from Event order by EventID;
END


GO
/****** Object:  StoredProcedure [dbo].[usp_EventStaticDelete]    Script Date: 5/15/2016 12:21:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[usp_EventStaticDelete] 
	-- Add the parameters for the stored procedure here
	@EventID int
AS
BEGIN
    -- Insert statements for procedure here
	Delete from Event where EventID = @EventID;
END


GO
/****** Object:  StoredProcedure [dbo].[usp_EventUpdate]    Script Date: 5/15/2016 12:21:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_EventUpdate] 
	-- Add the parameters for the stored procedure here
	@EventID int,
	@UserID int,
	@EventTitle nvarchar(20), 
	@EventDescription nvarchar(1000), 
	@EventDate date, 
	@ConcurrencyID int
AS
BEGIN
    -- Insert statements for procedure here
	Update Event set
		UserID = @UserID,
		EventTitle = @EventTitle, 
		EventDescription = @EventDescription,
		EventDate = @EventDate,
		ConcurrencyID = (@ConcurrencyID + 1)
	Where EventID = @EventID and ConcurrencyID = @ConcurrencyID;

END


GO
/****** Object:  StoredProcedure [dbo].[usp_testingResetData]    Script Date: 5/15/2016 12:21:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_testingResetData] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SET IDENTITY_INSERT [dbo].[Event] ON

	delete from Event

	INSERT INTO [dbo].[Event]
           ([EventId]
		   ,[UserId]
           ,[EventDate]
           ,[EventTitle]
           ,[EventDescription]
           ,[ConcurrencyId])
     VALUES
           (1, 1, '05-13-2016', 'First Event', 'This is my first test event', 1)

	INSERT INTO [dbo].[Event]
           ([EventId]
		   ,[UserId]
           ,[EventDate]
           ,[EventTitle]
           ,[EventDescription]
           ,[ConcurrencyId])
     VALUES
			(2, 3, '05-16-2016', 'Cats Birthday', 'Happy Birthday Cat!', 1)


	set identity_insert dbo.Event off
	DBCC CHECKIDENT ('Event', RESEED, 2)

End


GO
USE [master]
GO
ALTER DATABASE [EventCalendar] SET  READ_WRITE 
GO
