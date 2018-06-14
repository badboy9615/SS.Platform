
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/25/2018 21:13:24
-- Generated from EDMX file: F:\SS.Platform\SS.Platform.OA.Model\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [EnterprisePlatform];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_EmployeeFamily]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Family] DROP CONSTRAINT [FK_EmployeeFamily];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeTrain]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Train] DROP CONSTRAINT [FK_EmployeeTrain];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeExperience]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Experience] DROP CONSTRAINT [FK_EmployeeExperience];
GO
IF OBJECT_ID(N'[dbo].[FK_ModuleControl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Control] DROP CONSTRAINT [FK_ModuleControl];
GO
IF OBJECT_ID(N'[dbo].[FK_ControlActionInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ActionInfo] DROP CONSTRAINT [FK_ControlActionInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_ActionInfoMenuInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MenuInfo] DROP CONSTRAINT [FK_ActionInfoMenuInfo];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfo];
GO
IF OBJECT_ID(N'[dbo].[BaseEntity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BaseEntity];
GO
IF OBJECT_ID(N'[dbo].[Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee];
GO
IF OBJECT_ID(N'[dbo].[Family]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Family];
GO
IF OBJECT_ID(N'[dbo].[Train]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Train];
GO
IF OBJECT_ID(N'[dbo].[Experience]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Experience];
GO
IF OBJECT_ID(N'[dbo].[Module]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Module];
GO
IF OBJECT_ID(N'[dbo].[Control]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Control];
GO
IF OBJECT_ID(N'[dbo].[ActionInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActionInfo];
GO
IF OBJECT_ID(N'[dbo].[MenuInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MenuInfo];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserInfo'
CREATE TABLE [dbo].[UserInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(32)  NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [Pwd] nvarchar(32)  NOT NULL,
    [Mail] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [QQNum] nvarchar(max)  NULL,
    [SubBy] int  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [ModifiedBy] int  NOT NULL,
    [ModifiedTIme] datetime  NOT NULL,
    [TakeEffect] bit  NOT NULL,
    [TakeEffectTime] datetime  NOT NULL,
    [LoseEffectTime] datetime  NOT NULL,
    [DelFlag] smallint  NOT NULL,
    [Remark] nvarchar(256)  NULL
);
GO

-- Creating table 'BaseEntity'
CREATE TABLE [dbo].[BaseEntity] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SubBy] int  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [ModifiedBy] int  NOT NULL,
    [ModifiedTIme] datetime  NOT NULL,
    [DelFlag] smallint  NOT NULL,
    [TakeEffect] bit  NOT NULL,
    [TakeEffectTime] datetime  NOT NULL,
    [LoseEffectTime] datetime  NOT NULL,
    [Remark] nvarchar(256)  NULL,
    [Code] nvarchar(32)  NOT NULL,
    [Name] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'Employee'
CREATE TABLE [dbo].[Employee] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SubBy] int  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [ModifiedBy] int  NOT NULL,
    [ModifiedTIme] datetime  NOT NULL,
    [DelFlag] smallint  NOT NULL,
    [TakeEffect] bit  NOT NULL,
    [TakeEffectTime] datetime  NOT NULL,
    [LoseEffectTime] datetime  NOT NULL,
    [Remark] nvarchar(256)  NULL,
    [Code] nvarchar(32)  NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [xyd] int  NOT NULL,
    [dj] int  NULL,
    [xb] int  NOT NULL,
    [wh] int  NULL,
    [zzmm] int  NULL,
    [zp] nvarchar(max)  NULL,
    [nl] int  NOT NULL,
    [sx] int  NULL,
    [csrq] datetime  NULL,
    [hyzk] int  NULL,
    [sg] int  NULL,
    [qwgz] int  NULL,
    [grxy] int  NULL,
    [jkzk] int  NULL,
    [tz] int  NULL,
    [sfzj] bit  NULL,
    [xm] int  NULL,
    [yx] int  NULL,
    [zzqy] int  NULL,
    [zxsj] int  NULL,
    [gzfw] int  NULL,
    [gznx] int  NULL,
    [sfzh] nvarchar(max)  NULL,
    [sfzt] nvarchar(max)  NULL,
    [xxly] nvarchar(max)  NULL,
    [sjhm] nvarchar(max)  NULL,
    [jtzz] nvarchar(max)  NULL,
    [wxh] nvarchar(max)  NULL,
    [xzzz] nvarchar(max)  NULL,
    [bz] nvarchar(max)  NULL,
    [jkzs] bit  NOT NULL,
    [yxqz] datetime  NULL,
    [myhlz] bit  NOT NULL,
    [yyszgz] bit  NOT NULL,
    [lrsj] datetime  NOT NULL,
    [gzzt] int  NULL,
    [jbkh] nvarchar(max)  NULL,
    [qwjob] nvarchar(max)  NULL
);
GO

-- Creating table 'Family'
CREATE TABLE [dbo].[Family] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SubBy] int  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [ModifiedBy] int  NOT NULL,
    [ModifiedTIme] datetime  NOT NULL,
    [DelFlag] smallint  NOT NULL,
    [TakeEffect] bit  NOT NULL,
    [TakeEffectTime] datetime  NOT NULL,
    [LoseEffectTime] datetime  NOT NULL,
    [Remark] nvarchar(256)  NULL,
    [Code] nvarchar(32)  NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [Relation] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NULL,
    [EmployeeID] int  NOT NULL
);
GO

-- Creating table 'Train'
CREATE TABLE [dbo].[Train] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SubBy] int  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [ModifiedBy] int  NOT NULL,
    [ModifiedTIme] datetime  NOT NULL,
    [DelFlag] smallint  NOT NULL,
    [Remark] nvarchar(256)  NULL,
    [Name] nvarchar(32)  NOT NULL,
    [Pxsj] datetime  NOT NULL,
    [Pxqx] nvarchar(max)  NULL,
    [Pxlx] int  NOT NULL,
    [Code] nvarchar(32)  NOT NULL,
    [EmployeeID] int  NOT NULL
);
GO

-- Creating table 'Experience'
CREATE TABLE [dbo].[Experience] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SubBy] int  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [ModifiedBy] int  NOT NULL,
    [ModifiedTIme] datetime  NOT NULL,
    [DelFlag] smallint  NOT NULL,
    [TakeEffect] bit  NOT NULL,
    [TakeEffectTime] datetime  NOT NULL,
    [LoseEffectTime] datetime  NOT NULL,
    [Remark] nvarchar(256)  NULL,
    [Code] nvarchar(32)  NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [Qssj] datetime  NOT NULL,
    [Jssj] datetime  NOT NULL,
    [Khxm] nvarchar(32)  NULL,
    [Gzdd] nvarchar(32)  NULL,
    [Gzpd] nvarchar(32)  NULL,
    [EmployeeID] int  NOT NULL
);
GO

-- Creating table 'Module'
CREATE TABLE [dbo].[Module] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SubBy] int  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [ModifiedBy] int  NOT NULL,
    [ModifiedTIme] datetime  NOT NULL,
    [DelFlag] smallint  NOT NULL,
    [TakeEffect] bit  NOT NULL,
    [TakeEffectTime] datetime  NOT NULL,
    [LoseEffectTime] datetime  NOT NULL,
    [Remark] nvarchar(256)  NULL,
    [Code] nvarchar(32)  NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [Url] nvarchar(512)  NULL
);
GO

-- Creating table 'Control'
CREATE TABLE [dbo].[Control] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SubBy] int  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [ModifiedBy] int  NOT NULL,
    [ModifiedTIme] datetime  NOT NULL,
    [DelFlag] smallint  NOT NULL,
    [TakeEffect] bit  NOT NULL,
    [TakeEffectTime] datetime  NOT NULL,
    [LoseEffectTime] datetime  NOT NULL,
    [Remark] nvarchar(256)  NULL,
    [Code] nvarchar(32)  NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [Url] nvarchar(512)  NOT NULL,
    [ModuleID] int  NOT NULL
);
GO

-- Creating table 'ActionInfo'
CREATE TABLE [dbo].[ActionInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SubBy] int  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [ModifiedBy] int  NOT NULL,
    [ModifiedTIme] datetime  NOT NULL,
    [DelFlag] smallint  NOT NULL,
    [TakeEffect] bit  NOT NULL,
    [TakeEffectTime] datetime  NOT NULL,
    [LoseEffectTime] datetime  NOT NULL,
    [Remark] nvarchar(256)  NULL,
    [Code] nvarchar(32)  NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [Url] nvarchar(512)  NULL,
    [HttpMethod] varchar(32)  NULL,
    [ActionMethod] nvarchar(64)  NOT NULL,
    [ControlID] int  NOT NULL
);
GO

-- Creating table 'MenuInfo'
CREATE TABLE [dbo].[MenuInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SubBy] int  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [ModifiedBy] int  NOT NULL,
    [ModifiedTIme] datetime  NOT NULL,
    [DelFlag] smallint  NOT NULL,
    [TakeEffect] bit  NOT NULL,
    [TakeEffectTime] datetime  NOT NULL,
    [LoseEffectTime] datetime  NOT NULL,
    [Remark] nvarchar(256)  NULL,
    [Code] nvarchar(32)  NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [Sort] int  NULL,
    [IsVisable] bit  NOT NULL,
    [Height] int  NULL,
    [Width] int  NULL,
    [IconUrl] nvarchar(512)  NULL,
    [ParentId] int  NOT NULL,
    [ActionInfo_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'UserInfo'
ALTER TABLE [dbo].[UserInfo]
ADD CONSTRAINT [PK_UserInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'BaseEntity'
ALTER TABLE [dbo].[BaseEntity]
ADD CONSTRAINT [PK_BaseEntity]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Employee'
ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT [PK_Employee]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Family'
ALTER TABLE [dbo].[Family]
ADD CONSTRAINT [PK_Family]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Train'
ALTER TABLE [dbo].[Train]
ADD CONSTRAINT [PK_Train]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Experience'
ALTER TABLE [dbo].[Experience]
ADD CONSTRAINT [PK_Experience]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Module'
ALTER TABLE [dbo].[Module]
ADD CONSTRAINT [PK_Module]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Control'
ALTER TABLE [dbo].[Control]
ADD CONSTRAINT [PK_Control]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ActionInfo'
ALTER TABLE [dbo].[ActionInfo]
ADD CONSTRAINT [PK_ActionInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'MenuInfo'
ALTER TABLE [dbo].[MenuInfo]
ADD CONSTRAINT [PK_MenuInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [EmployeeID] in table 'Family'
ALTER TABLE [dbo].[Family]
ADD CONSTRAINT [FK_EmployeeFamily]
    FOREIGN KEY ([EmployeeID])
    REFERENCES [dbo].[Employee]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeFamily'
CREATE INDEX [IX_FK_EmployeeFamily]
ON [dbo].[Family]
    ([EmployeeID]);
GO

-- Creating foreign key on [EmployeeID] in table 'Train'
ALTER TABLE [dbo].[Train]
ADD CONSTRAINT [FK_EmployeeTrain]
    FOREIGN KEY ([EmployeeID])
    REFERENCES [dbo].[Employee]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeTrain'
CREATE INDEX [IX_FK_EmployeeTrain]
ON [dbo].[Train]
    ([EmployeeID]);
GO

-- Creating foreign key on [EmployeeID] in table 'Experience'
ALTER TABLE [dbo].[Experience]
ADD CONSTRAINT [FK_EmployeeExperience]
    FOREIGN KEY ([EmployeeID])
    REFERENCES [dbo].[Employee]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeExperience'
CREATE INDEX [IX_FK_EmployeeExperience]
ON [dbo].[Experience]
    ([EmployeeID]);
GO

-- Creating foreign key on [ModuleID] in table 'Control'
ALTER TABLE [dbo].[Control]
ADD CONSTRAINT [FK_ModuleControl]
    FOREIGN KEY ([ModuleID])
    REFERENCES [dbo].[Module]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModuleControl'
CREATE INDEX [IX_FK_ModuleControl]
ON [dbo].[Control]
    ([ModuleID]);
GO

-- Creating foreign key on [ControlID] in table 'ActionInfo'
ALTER TABLE [dbo].[ActionInfo]
ADD CONSTRAINT [FK_ControlActionInfo]
    FOREIGN KEY ([ControlID])
    REFERENCES [dbo].[Control]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ControlActionInfo'
CREATE INDEX [IX_FK_ControlActionInfo]
ON [dbo].[ActionInfo]
    ([ControlID]);
GO

-- Creating foreign key on [ActionInfo_ID] in table 'MenuInfo'
ALTER TABLE [dbo].[MenuInfo]
ADD CONSTRAINT [FK_ActionInfoMenuInfo]
    FOREIGN KEY ([ActionInfo_ID])
    REFERENCES [dbo].[ActionInfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ActionInfoMenuInfo'
CREATE INDEX [IX_FK_ActionInfoMenuInfo]
ON [dbo].[MenuInfo]
    ([ActionInfo_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------