
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/19/2017 20:48:07
-- Generated from EDMX file: E:\Document\Visual Studio 2017\Projects\WebApplication1\WebApplication1\Models\Competition.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Competition];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CompanyCompetitor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Competitors] DROP CONSTRAINT [FK_CompanyCompetitor];
GO
IF OBJECT_ID(N'[dbo].[FK_CompetitorScore]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Scores] DROP CONSTRAINT [FK_CompetitorScore];
GO
IF OBJECT_ID(N'[dbo].[FK_EventScore]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Scores] DROP CONSTRAINT [FK_EventScore];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyJudge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Judges] DROP CONSTRAINT [FK_CompanyJudge];
GO
IF OBJECT_ID(N'[dbo].[FK_EventJudge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Judges] DROP CONSTRAINT [FK_EventJudge];
GO
IF OBJECT_ID(N'[dbo].[FK_JudgeScore]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Scores] DROP CONSTRAINT [FK_JudgeScore];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Companys]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companys];
GO
IF OBJECT_ID(N'[dbo].[Competitors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Competitors];
GO
IF OBJECT_ID(N'[dbo].[Events]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Events];
GO
IF OBJECT_ID(N'[dbo].[Scores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Scores];
GO
IF OBJECT_ID(N'[dbo].[Judges]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Judges];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Companys'
CREATE TABLE [dbo].[Companys] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ACtype] nvarchar(max)  NULL
);
GO

-- Creating table 'Competitors'
CREATE TABLE [dbo].[Competitors] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Pro] nvarchar(max)  NOT NULL,
    [CompanyId] int  NOT NULL
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Pro] nvarchar(10)  NULL
);
GO

-- Creating table 'Scores'
CREATE TABLE [dbo].[Scores] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EventId] int  NOT NULL,
    [TimeConsume] time  NULL,
    [TimePenalty] time  NULL,
    [CompetitorId] int  NOT NULL,
    [JudgeTime] datetime  NULL,
    [ModifyTime] datetime  NULL,
    [JudgeId] int  NOT NULL,
    [Mark] float  NULL
);
GO

-- Creating table 'Judges'
CREATE TABLE [dbo].[Judges] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(10)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [CompanyId] int  NULL,
    [EventId] int  NULL,
    [StaffId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Companys'
ALTER TABLE [dbo].[Companys]
ADD CONSTRAINT [PK_Companys]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Competitors'
ALTER TABLE [dbo].[Competitors]
ADD CONSTRAINT [PK_Competitors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [PK_Events]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Scores'
ALTER TABLE [dbo].[Scores]
ADD CONSTRAINT [PK_Scores]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Judges'
ALTER TABLE [dbo].[Judges]
ADD CONSTRAINT [PK_Judges]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CompanyId] in table 'Competitors'
ALTER TABLE [dbo].[Competitors]
ADD CONSTRAINT [FK_CompanyCompetitor]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Companys]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyCompetitor'
CREATE INDEX [IX_FK_CompanyCompetitor]
ON [dbo].[Competitors]
    ([CompanyId]);
GO

-- Creating foreign key on [CompetitorId] in table 'Scores'
ALTER TABLE [dbo].[Scores]
ADD CONSTRAINT [FK_CompetitorScore]
    FOREIGN KEY ([CompetitorId])
    REFERENCES [dbo].[Competitors]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompetitorScore'
CREATE INDEX [IX_FK_CompetitorScore]
ON [dbo].[Scores]
    ([CompetitorId]);
GO

-- Creating foreign key on [EventId] in table 'Scores'
ALTER TABLE [dbo].[Scores]
ADD CONSTRAINT [FK_EventScore]
    FOREIGN KEY ([EventId])
    REFERENCES [dbo].[Events]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventScore'
CREATE INDEX [IX_FK_EventScore]
ON [dbo].[Scores]
    ([EventId]);
GO

-- Creating foreign key on [CompanyId] in table 'Judges'
ALTER TABLE [dbo].[Judges]
ADD CONSTRAINT [FK_CompanyJudge]
    FOREIGN KEY ([CompanyId])
    REFERENCES [dbo].[Companys]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyJudge'
CREATE INDEX [IX_FK_CompanyJudge]
ON [dbo].[Judges]
    ([CompanyId]);
GO

-- Creating foreign key on [EventId] in table 'Judges'
ALTER TABLE [dbo].[Judges]
ADD CONSTRAINT [FK_EventJudge]
    FOREIGN KEY ([EventId])
    REFERENCES [dbo].[Events]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EventJudge'
CREATE INDEX [IX_FK_EventJudge]
ON [dbo].[Judges]
    ([EventId]);
GO

-- Creating foreign key on [JudgeId] in table 'Scores'
ALTER TABLE [dbo].[Scores]
ADD CONSTRAINT [FK_JudgeScore]
    FOREIGN KEY ([JudgeId])
    REFERENCES [dbo].[Judges]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_JudgeScore'
CREATE INDEX [IX_FK_JudgeScore]
ON [dbo].[Scores]
    ([JudgeId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------