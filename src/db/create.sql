-- Set your preferred database-name here
USE [database_name]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[get_type](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[get_type_name] [nvarchar](10) NULL,
	[get_type_hour] [int] NULL,
	[get_type_minute] [int] NULL,
	[get_enabled] [bit] NULL,
    PRIMARY KEY CLUSTERED 
    ([id] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY]

CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](250) NULL,
	[userid] [int] NOT NULL,
	[languagecode] [nvarchar](100) NULL,
	[firstname] [nvarchar](100) NULL,
	[lastname] [nvarchar](100) NULL,
    PRIMARY KEY CLUSTERED 
    ([userid] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY]

CREATE TABLE [dbo].[botget](
	[messageid] [int] NOT NULL,
	[userid] [int] NULL,
	[get_date] [datetime] NULL,
	[get_seconds] [int] NULL,
	[get_weekday] [nvarchar](20) NULL,
	[get_type_id] [int] NULL,
	[chatid] [int] NULL,
	[chattitle] [nvarchar](150) NULL,
    PRIMARY KEY CLUSTERED ([messageid] ASC)
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY]

ALTER TABLE [dbo].[botget]  WITH CHECK ADD FOREIGN KEY([get_type_id])
REFERENCES [dbo].[get_type] ([id])

ALTER TABLE [dbo].[botget]  WITH CHECK ADD FOREIGN KEY([userid])
REFERENCES [dbo].[users] ([userid])
