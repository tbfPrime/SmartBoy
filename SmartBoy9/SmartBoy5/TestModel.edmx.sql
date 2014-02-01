create database SmartBoyDatabase1
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/06/2013 00:44:10
-- Generated from EDMX file: C:\Users\TBF\Desktop\SmartBoy\SmartBoy9\SmartBoy5\TestModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SmartBoyDatabase1];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ID_SB_To_Album_SB]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Track_Album_Reln] DROP CONSTRAINT [FK_ID_SB_To_Album_SB];
GO
IF OBJECT_ID(N'[dbo].[FK_ID_SB_To_Artist_SB]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Track_Artist_Reln] DROP CONSTRAINT [FK_ID_SB_To_Artist_SB];
GO
IF OBJECT_ID(N'[dbo].[FK_ID_SB_To_Track]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ID_SB] DROP CONSTRAINT [FK_ID_SB_To_Track];
GO
IF OBJECT_ID(N'[dbo].[FK_ID_SB_To_Track_SB]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Track_Album_Reln] DROP CONSTRAINT [FK_ID_SB_To_Track_SB];
GO
IF OBJECT_ID(N'[dbo].[FK_ID_SB_To_Track_SB1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Track_Artist_Reln] DROP CONSTRAINT [FK_ID_SB_To_Track_SB1];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Album_SB]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Album_SB];
GO
IF OBJECT_ID(N'[dbo].[Artist_SB]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Artist_SB];
GO
IF OBJECT_ID(N'[dbo].[Composer_SB]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Composer_SB];
GO
IF OBJECT_ID(N'[dbo].[Conductor_SB]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Conductor_SB];
GO
IF OBJECT_ID(N'[dbo].[Genre_SB]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Genre_SB];
GO
IF OBJECT_ID(N'[dbo].[ID_SB]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ID_SB];
GO
IF OBJECT_ID(N'[dbo].[Picture_SB]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Picture_SB];
GO
IF OBJECT_ID(N'[dbo].[Track_Album_Reln]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Track_Album_Reln];
GO
IF OBJECT_ID(N'[dbo].[Track_Artist_Reln]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Track_Artist_Reln];
GO
IF OBJECT_ID(N'[dbo].[Track_SB]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Track_SB];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Album_SB'
CREATE TABLE [dbo].[Album_SB] (
    [MB_Release_ID] varchar(50)  NOT NULL,
    [Album_Name] varchar(max)  NULL,
    [Release_Date] datetime  NULL,
    [Release_Year] varchar(4)  NULL,
    [Album_Awards] varchar(max)  NULL,
    [Album_Links] varchar(max)  NULL,
    [Album_Content] varchar(max)  NULL,
    [Album_Image_ID] int  NULL,
    [Album_Type] varchar(20)  NULL,
    [Album_Status] varchar(20)  NULL,
    [Album_Quality] varchar(20)  NULL,
    [Album_Packaging] varchar(50)  NULL,
    [Album_Language] varchar(20)  NULL,
    [Album_Country] varchar(50)  NULL,
    [Album_Lablel] varchar(50)  NULL,
    [Label_Disambiguation] varchar(50)  NULL,
    [Album_Script] varchar(20)  NULL,
    [Album_Barcode] varchar(20)  NULL
);
GO

-- Creating table 'Artist_SB'
CREATE TABLE [dbo].[Artist_SB] (
    [MB_Artist_ID] varchar(50)  NOT NULL,
    [Artist_Name] varchar(50)  NULL,
    [Artist_Type] varchar(20)  NULL,
    [Artist_DOB] datetime  NULL,
    [Artist_DOD] datetime  NULL,
    [Artist_Begin] varchar(4)  NULL,
    [Artist_Gender] varchar(10)  NULL,
    [Artist_Country] varchar(50)  NULL,
    [Artist_Awards] varchar(max)  NULL,
    [Artist_Links] varchar(max)  NULL,
    [Artist_Content] varchar(max)  NULL,
    [Artist_Image_ID] int  NULL
);
GO

-- Creating table 'Composer_SB'
CREATE TABLE [dbo].[Composer_SB] (
    [Composer_ID] int  NOT NULL,
    [Composer_Name] varchar(50)  NULL,
    [Composer_DOB] datetime  NULL,
    [Composer_DOD] datetime  NULL,
    [Composer_Country] varchar(50)  NULL,
    [Composer_Awards] varchar(max)  NULL,
    [Composer_Links] varchar(max)  NULL,
    [Composer_Content] varchar(max)  NULL,
    [Composer_Image_ID] int  NULL
);
GO

-- Creating table 'Conductor_SB'
CREATE TABLE [dbo].[Conductor_SB] (
    [Conductor_ID] int  NOT NULL,
    [Conductor_Name] varchar(50)  NULL,
    [Conductor_DOB] datetime  NULL,
    [Conductor_DOD] datetime  NULL,
    [Conductor_Country] varchar(50)  NULL,
    [Conductor_Awards] varchar(max)  NULL,
    [Conductor_Links] varchar(max)  NULL,
    [Conductor_Content] varchar(max)  NULL,
    [Conductor_Image_ID] int  NULL
);
GO

-- Creating table 'Genre_SB'
CREATE TABLE [dbo].[Genre_SB] (
    [Genre_ID] int  NOT NULL,
    [Genre_Type] varchar(50)  NULL,
    [Genre_Description] varchar(max)  NULL
);
GO

-- Creating table 'ID_SB'
CREATE TABLE [dbo].[ID_SB] (
    [Hash] varchar(50)  NOT NULL,
    [MB_Track_ID] varchar(50)  NULL,
    [Fingerprint] varchar(max)  NULL,
    [Duration] varchar(10)  NULL,
    [FilePath] varchar(max)  NULL
);
GO

-- Creating table 'Picture_SB'
CREATE TABLE [dbo].[Picture_SB] (
    [Picture_ID] int  NOT NULL,
    [Image] varbinary(max)  NULL
);
GO

-- Creating table 'Track_Album_Reln'
CREATE TABLE [dbo].[Track_Album_Reln] (
    [id] varchar(100)  NOT NULL,
    [MB_Track_ID] varchar(50)  NULL,
    [MB_AlbumID] varchar(50)  NULL
);
GO

-- Creating table 'Track_Artist_Reln'
CREATE TABLE [dbo].[Track_Artist_Reln] (
    [id] varchar(100)  NOT NULL,
    [MB_Track_ID] varchar(50)  NULL,
    [MB_ArtistID] varchar(50)  NULL
);
GO

-- Creating table 'Track_SB'
CREATE TABLE [dbo].[Track_SB] (
    [MB_TrackID] varchar(50)  NOT NULL,
    [Title] varchar(max)  NULL,
    [Counter] int  NULL,
    [MB_Release_Type] varchar(50)  NULL,
    [Track_Content] varchar(max)  NULL,
    [Lyrics] varchar(max)  NULL,
    [Track_Image_ID] int  NULL,
    [Track] int  NULL,
    [Track_Count] int  NULL,
    [Disc] int  NULL,
    [Disc_Count] int  NULL,
    [MusicIP_ID] varchar(50)  NULL,
    [Amazon_ID] int  NULL,
    [Track_length] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MB_Release_ID] in table 'Album_SB'
ALTER TABLE [dbo].[Album_SB]
ADD CONSTRAINT [PK_Album_SB]
    PRIMARY KEY CLUSTERED ([MB_Release_ID] ASC);
GO

-- Creating primary key on [MB_Artist_ID] in table 'Artist_SB'
ALTER TABLE [dbo].[Artist_SB]
ADD CONSTRAINT [PK_Artist_SB]
    PRIMARY KEY CLUSTERED ([MB_Artist_ID] ASC);
GO

-- Creating primary key on [Composer_ID] in table 'Composer_SB'
ALTER TABLE [dbo].[Composer_SB]
ADD CONSTRAINT [PK_Composer_SB]
    PRIMARY KEY CLUSTERED ([Composer_ID] ASC);
GO

-- Creating primary key on [Conductor_ID] in table 'Conductor_SB'
ALTER TABLE [dbo].[Conductor_SB]
ADD CONSTRAINT [PK_Conductor_SB]
    PRIMARY KEY CLUSTERED ([Conductor_ID] ASC);
GO

-- Creating primary key on [Genre_ID] in table 'Genre_SB'
ALTER TABLE [dbo].[Genre_SB]
ADD CONSTRAINT [PK_Genre_SB]
    PRIMARY KEY CLUSTERED ([Genre_ID] ASC);
GO

-- Creating primary key on [Hash] in table 'ID_SB'
ALTER TABLE [dbo].[ID_SB]
ADD CONSTRAINT [PK_ID_SB]
    PRIMARY KEY CLUSTERED ([Hash] ASC);
GO

-- Creating primary key on [Picture_ID] in table 'Picture_SB'
ALTER TABLE [dbo].[Picture_SB]
ADD CONSTRAINT [PK_Picture_SB]
    PRIMARY KEY CLUSTERED ([Picture_ID] ASC);
GO

-- Creating primary key on [id] in table 'Track_Album_Reln'
ALTER TABLE [dbo].[Track_Album_Reln]
ADD CONSTRAINT [PK_Track_Album_Reln]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Track_Artist_Reln'
ALTER TABLE [dbo].[Track_Artist_Reln]
ADD CONSTRAINT [PK_Track_Artist_Reln]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [MB_TrackID] in table 'Track_SB'
ALTER TABLE [dbo].[Track_SB]
ADD CONSTRAINT [PK_Track_SB]
    PRIMARY KEY CLUSTERED ([MB_TrackID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MB_AlbumID] in table 'Track_Album_Reln'
ALTER TABLE [dbo].[Track_Album_Reln]
ADD CONSTRAINT [FK_ID_SB_To_Album_SB]
    FOREIGN KEY ([MB_AlbumID])
    REFERENCES [dbo].[Album_SB]
        ([MB_Release_ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ID_SB_To_Album_SB'
CREATE INDEX [IX_FK_ID_SB_To_Album_SB]
ON [dbo].[Track_Album_Reln]
    ([MB_AlbumID]);
GO

-- Creating foreign key on [MB_ArtistID] in table 'Track_Artist_Reln'
ALTER TABLE [dbo].[Track_Artist_Reln]
ADD CONSTRAINT [FK_ID_SB_To_Artist_SB]
    FOREIGN KEY ([MB_ArtistID])
    REFERENCES [dbo].[Artist_SB]
        ([MB_Artist_ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ID_SB_To_Artist_SB'
CREATE INDEX [IX_FK_ID_SB_To_Artist_SB]
ON [dbo].[Track_Artist_Reln]
    ([MB_ArtistID]);
GO

-- Creating foreign key on [MB_Track_ID] in table 'ID_SB'
ALTER TABLE [dbo].[ID_SB]
ADD CONSTRAINT [FK_ID_SB_To_Track]
    FOREIGN KEY ([MB_Track_ID])
    REFERENCES [dbo].[Track_SB]
        ([MB_TrackID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ID_SB_To_Track'
CREATE INDEX [IX_FK_ID_SB_To_Track]
ON [dbo].[ID_SB]
    ([MB_Track_ID]);
GO

-- Creating foreign key on [MB_Track_ID] in table 'Track_Album_Reln'
ALTER TABLE [dbo].[Track_Album_Reln]
ADD CONSTRAINT [FK_ID_SB_To_Track_SB]
    FOREIGN KEY ([MB_Track_ID])
    REFERENCES [dbo].[Track_SB]
        ([MB_TrackID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ID_SB_To_Track_SB'
CREATE INDEX [IX_FK_ID_SB_To_Track_SB]
ON [dbo].[Track_Album_Reln]
    ([MB_Track_ID]);
GO

-- Creating foreign key on [MB_Track_ID] in table 'Track_Artist_Reln'
ALTER TABLE [dbo].[Track_Artist_Reln]
ADD CONSTRAINT [FK_ID_SB_To_Track_SB1]
    FOREIGN KEY ([MB_Track_ID])
    REFERENCES [dbo].[Track_SB]
        ([MB_TrackID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ID_SB_To_Track_SB1'
CREATE INDEX [IX_FK_ID_SB_To_Track_SB1]
ON [dbo].[Track_Artist_Reln]
    ([MB_Track_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------