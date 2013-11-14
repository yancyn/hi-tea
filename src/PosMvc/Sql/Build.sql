USE [Master]
IF NOT EXISTS(SELECT name FROM master.dbo.sysdatabases WHERE name = 'Pos')
CREATE database [Pos]
GO

Drop table OrderSubItems;
Drop table SubMenus;
Drop table OrderItems;
Drop table Orders;
Drop table OrderTypes;
Drop table Charges;
Drop table Menus;
Drop table Statuses;
Drop table Categories;
Drop table Users;
Drop table Roles;


USE [Pos]

-- Create user Role table
-- Admin, Cashier, Staff, Guest
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Roles]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
CREATE TABLE [Roles] (
        [Id]                                    smallint IDENTITY NOT NULL,
        [Name]                                  nvarchar(15) NOT NULL
CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
)
GO

-- Create Users table
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Users]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
CREATE TABLE [Users] (
        [Id]                                    int IDENTITY NOT NULL,
        [RoleId]								smallint NOT NULL,
		[Username]                              nvarchar(30) NOT NULL,
        [Password]                              char(128) NOT NULL, --stored hash instead of pure text

        [DisplayName]							nvarchar(100),
		[Telephone]								nvarchar(100),
		[Mobile]								nvarchar(100),

		[Street1]                               nvarchar(100),
        [Street2]                               nvarchar(100),
        [City]                                  nvarchar(100),
        [Postcode]                              nvarchar(100),
		[State]									nvarchar(100),
		[Country]								nvarchar(100),

		[Point]									int NOT NULL DEFAULT 0,
		[Active]								bit NOT NULL DEFAULT 1
CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
CONSTRAINT [FK_Users_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id]),
)
GO

-- Create Categories table
-- Set,Food,Snack,Drink,Dessert
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Categories]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
CREATE TABLE [Categories] (
        [Id]                                    smallint IDENTITY NOT NULL,
        [Name]									nvarchar(30) NOT NULL
CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
)
GO

-- Create Menu table
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Menus]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
CREATE TABLE [Menus] (
        [Id]                                    int IDENTITY NOT NULL,
        [CategoryId]							smallint NOT NULL,
		[Code]									nvarchar(50) NOT NULL,
		[Name]									nvarchar(100),
		[Description]							ntext,
        [Image]                                 nvarchar(255),  --stored image url
        [Price]                                 money NOT NULL DEFAULT 0,
		[Active]								bit NOT NULL DEFAULT 1
CONSTRAINT [PK_Menus] PRIMARY KEY ([Id]),
CONSTRAINT [FK_Menus_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id])
)
GO

-- Create Submenu table
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SubMenus]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
CREATE TABLE [SubMenus] (
        [Id]                                    int IDENTITY NOT NULL,
        [ParentId]								int NOT NULL,
		[Name]									nvarchar(100),
        [Price]                                 money NOT NULL DEFAULT 0,
		[Active]								bit NOT NULL DEFAULT 1
CONSTRAINT [PK_SubMenus] PRIMARY KEY ([Id]),
CONSTRAINT [FK_SubMenus_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Menus]([Id])
)
GO

-- create order type table dine in or out
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[OrderTypes]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
CREATE TABLE [OrderTypes] (
        [Id]                                    smallint IDENTITY NOT NULL,
        [Name]									nvarchar(30) NOT NULL
CONSTRAINT [PK_OrderTypes] PRIMARY KEY ([Id])
)
GO

-- confirm,done
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Statuses]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
CREATE TABLE [Statuses] (
        [Id]                                    smallint IDENTITY NOT NULL,
        [Name]									nvarchar(30) NOT NULL
CONSTRAINT [PK_Statuses] PRIMARY KEY ([Id])
)
GO

-- Create Order table
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Orders]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
CREATE TABLE [Orders] (
        [Id]                                    int IDENTITY NOT NULL,
		[CreatedById]							int NOT NULL,
		[Created]								datetime NOT NULL,
		[ReceiptDate]							datetime,
		[QueueNo]								varchar(10),
		[TableNo]								varchar(10),
		[MemberId]								int,
		[Total]									money NOT NULL DEFAULT 0
CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
CONSTRAINT [FK_Orders_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Users]([Id]),
CONSTRAINT [FK_Orders_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Users]([Id])
)
GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[OrderItems]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
CREATE TABLE [OrderItems] (
        [Id]                                    int IDENTITY NOT NULL,
		[ParentId]								int NOT NULL,
		[OrderTypeId]							smallint NOT NULL,
		[MenuId]								int NOT NULL,
		[StatusId]								smallint NOT NULL
CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
CONSTRAINT [FK_OrderItems_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Orders]([Id]),
CONSTRAINT [FK_Orders_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [Menus]([Id]),
CONSTRAINT [FK_Orders_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Statuses]([Id])
)
GO

-- stored charge percentages
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Charges]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
CREATE TABLE [Charges] (
        [Id]                                    smallint IDENTITY NOT NULL,
        [Name]									nvarchar(30) NOT NULL,
		[Value]									decimal NOT NULL DEFAULT 0,
		[Active]								bit NOT NULL DEFAULT 1
CONSTRAINT [PK_Charges] PRIMARY KEY ([Id])
)
GO



-- Initial default data
INSERT INTO [Roles](Name) VALUES ('Admin');
INSERT INTO [Roles](Name) VALUES ('Cashier');
INSERT INTO [Roles](Name) VALUES ('Staff');
INSERT INTO [Roles](Name) VALUES ('Guest');

INSERT INTO [Categories](Name) VALUES ('Set Menu');
INSERT INTO [Categories](Name) VALUES ('Food');
INSERT INTO [Categories](Name) VALUES ('Snack');
INSERT INTO [Categories](Name) VALUES ('Drink');
INSERT INTO [Categories](Name) VALUES ('Dessert');

INSERT INTO [OrderTypes](Name) VALUES ('Dine In');
INSERT INTO [OrderTypes](Name) VALUES ('Take Out');

INSERT INTO [Statuses](Name) VALUES ('Confirm');
INSERT INTO [Statuses](Name) VALUES ('Complete');

INSERT INTO [Charges](Name,Value,Active) values('Govn 6%',0.06,1);

INSERT INTO [Users](Username,Password,RoleId,DisplayName,Point,Active) VALUES('admin','admin',1,'admin',0,1);
INSERT INTO [Users](Username,Password,RoleId,DisplayName,Point,Active) VALUES('cashier','cashier',2,'cashier',0,1);