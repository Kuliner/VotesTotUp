
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/19/2016 23:14:57
-- Generated from EDMX file: C:\Users\Rafal\Documents\Visual Studio 2015\Projects\VotesTotUp\VotesTotUp\DbModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [VotersDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_VoterCandidate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VoterSet] DROP CONSTRAINT [FK_VoterCandidate];
GO
IF OBJECT_ID(N'[dbo].[FK_PartyCandidate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CandidateSet] DROP CONSTRAINT [FK_PartyCandidate];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CandidateSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CandidateSet];
GO
IF OBJECT_ID(N'[dbo].[PartySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PartySet];
GO
IF OBJECT_ID(N'[dbo].[VoterSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VoterSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CandidateSet'
CREATE TABLE [dbo].[CandidateSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Party_Id] int  NOT NULL
);
GO

-- Creating table 'PartySet'
CREATE TABLE [dbo].[PartySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'VoterSet'
CREATE TABLE [dbo].[VoterSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Pesel] nvarchar(max)  NOT NULL,
    [Voted] bit  NOT NULL,
    [Candidate_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'CandidateSet'
ALTER TABLE [dbo].[CandidateSet]
ADD CONSTRAINT [PK_CandidateSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PartySet'
ALTER TABLE [dbo].[PartySet]
ADD CONSTRAINT [PK_PartySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VoterSet'
ALTER TABLE [dbo].[VoterSet]
ADD CONSTRAINT [PK_VoterSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Candidate_Id] in table 'VoterSet'
ALTER TABLE [dbo].[VoterSet]
ADD CONSTRAINT [FK_VoterCandidate]
    FOREIGN KEY ([Candidate_Id])
    REFERENCES [dbo].[CandidateSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VoterCandidate'
CREATE INDEX [IX_FK_VoterCandidate]
ON [dbo].[VoterSet]
    ([Candidate_Id]);
GO

-- Creating foreign key on [Party_Id] in table 'CandidateSet'
ALTER TABLE [dbo].[CandidateSet]
ADD CONSTRAINT [FK_PartyCandidate]
    FOREIGN KEY ([Party_Id])
    REFERENCES [dbo].[PartySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PartyCandidate'
CREATE INDEX [IX_FK_PartyCandidate]
ON [dbo].[CandidateSet]
    ([Party_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------