
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/21/2016 16:57:08
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

IF OBJECT_ID(N'[dbo].[FK_PartyCandidate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CandidateSet] DROP CONSTRAINT [FK_PartyCandidate];
GO
IF OBJECT_ID(N'[dbo].[FK_VoterCandidate_Voter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VoterCandidate] DROP CONSTRAINT [FK_VoterCandidate_Voter];
GO
IF OBJECT_ID(N'[dbo].[FK_VoterCandidate_Candidate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VoterCandidate] DROP CONSTRAINT [FK_VoterCandidate_Candidate];
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
IF OBJECT_ID(N'[dbo].[VoterCandidate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VoterCandidate];
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
    [VoteValid] bit  NOT NULL
);
GO

-- Creating table 'Statistics'
CREATE TABLE [dbo].[Statistics] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BlockedAttempts] int  NOT NULL
);
GO

-- Creating table 'VoterCandidate'
CREATE TABLE [dbo].[VoterCandidate] (
    [Voters_Id] int  NOT NULL,
    [Candidates_Id] int  NOT NULL
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

-- Creating primary key on [Id] in table 'Statistics'
ALTER TABLE [dbo].[Statistics]
ADD CONSTRAINT [PK_Statistics]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Voters_Id], [Candidates_Id] in table 'VoterCandidate'
ALTER TABLE [dbo].[VoterCandidate]
ADD CONSTRAINT [PK_VoterCandidate]
    PRIMARY KEY CLUSTERED ([Voters_Id], [Candidates_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

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

-- Creating foreign key on [Voters_Id] in table 'VoterCandidate'
ALTER TABLE [dbo].[VoterCandidate]
ADD CONSTRAINT [FK_VoterCandidate_Voter]
    FOREIGN KEY ([Voters_Id])
    REFERENCES [dbo].[VoterSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Candidates_Id] in table 'VoterCandidate'
ALTER TABLE [dbo].[VoterCandidate]
ADD CONSTRAINT [FK_VoterCandidate_Candidate]
    FOREIGN KEY ([Candidates_Id])
    REFERENCES [dbo].[CandidateSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VoterCandidate_Candidate'
CREATE INDEX [IX_FK_VoterCandidate_Candidate]
ON [dbo].[VoterCandidate]
    ([Candidates_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------