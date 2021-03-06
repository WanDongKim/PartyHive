CREATE DATABASE [PartyHive]

USE [PartyHive]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 2019-03-17 2:40:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[BookingID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[PartyID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BookingID] ASC,
	[UserID] ASC,
	[PartyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Host]    Script Date: 2019-03-17 2:40:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Host](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Phone] [varchar](100) NOT NULL,
	[Address] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Host] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Party]    Script Date: 2019-03-17 2:40:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Party](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [VARCHAR] NOT NULL,
	[Price] [numeric](18, 2) NULL,
	[Address] [varchar](50) NOT NULL,
	[TargetAudience] [varchar](50) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[CurrentEnrollment] [varchar](50) NULL,
	[MaxEnrollment] [varchar](50) NOT NULL,
	[IsActivated] [bit] NOT NULL,
	[HostID] [int] NOT NULL,
	[DateTime] DATETIME NULL, 
 CONSTRAINT [PK_Party] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2019-03-17 2:40:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Phone] [varchar](100) NOT NULL,
	[Address] [varchar](50) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Party_Booking] FOREIGN KEY([PartyID])
REFERENCES [dbo].[Party] ([ID])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Party_Booking]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_User_Booking] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_User_Booking]
GO
ALTER TABLE [dbo].[Party]  WITH CHECK ADD  CONSTRAINT [FK_Host_PostParty] FOREIGN KEY([HostID])
REFERENCES [dbo].[Host] ([ID])
GO
ALTER TABLE [dbo].[Party] CHECK CONSTRAINT [FK_Host_PostParty]
GO

CREATE TABLE [dbo].[Comment] (
    [CommentId] INT           IDENTITY (1, 1) NOT NULL,
    [Body]      VARCHAR (100) NOT NULL,
    [UserID]    INT           NOT NULL,
    [PartyID]   INT           NOT NULL,
    [Title] VARCHAR(50) NULL, 
    PRIMARY KEY CLUSTERED ([CommentId] ASC, [UserID] ASC, [PartyID] ASC),
    CONSTRAINT [FK_Party_Comment] FOREIGN KEY ([PartyID]) REFERENCES [dbo].[Party] ([ID]),
    CONSTRAINT [FK_User_Comment] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([ID])
);