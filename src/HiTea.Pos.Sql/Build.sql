-- This is SQLite build script
PRAGMA foreign_keys = ON;

--drop table OrderItems;
--drop table Orders;
--drop table OrderTypes;
--drop table Charges;
--drop table Menus;
--drop table Statuses;
--drop table Categories;
--drop table Users;
--drop table Roles;

CREATE TABLE IF NOT EXISTS Roles(
	Id integer primary key autoincrement,
	Name varchar(15)
);

CREATE TABLE IF NOT EXISTS Users(
	Id integer primary key autoincrement,
	Username varchar(30),
	Password text,
	RoleId integer,
	Displayname text,
	Street1 text,
	Street2 text,
	Postcode text,
	City text,
	State text,
	Country text,
	Point integer,
	foreign key(RoleId) references Roles(Id)
);

CREATE TABLE IF NOT EXISTS Categories(
	Id integer primary key autoincrement,
	Name varchar(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS Statuses(
	Id integer primary key autoincrement,
	Name varchar(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS Menus(
	Id	integer primary key autoincrement,
	CategoryId integer,
	Code text NOT NULL,
	Name text,
	Price real,
	foreign key(CategoryId) references Categories(Id)
);

-- stored charge percentage
CREATE TABLE IF NOT EXISTS Charges(
	Id integer primary key autoincrement,
	Name varchar(30),
	Value real
);

CREATE TABLE IF NOT EXISTS OrderTypes(
	Id integer primary key autoincrement,
	Name varchar(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS Orders(
	Id integer primary key autoincrement,
	CreatedById integer NOT NULL,
	Created datetime NOT NULL,
	DODate datetime,
	ReceiptDate datetime,
	TableNo text,
	MemberId integer,
	Total real NOT NULL,
	foreign key(CreatedById) references Users(Id)
);

CREATE TABLE IF NOT EXISTS OrderItems(
	Id integer primary key autoincrement,
	ParentId integer not null,
	MenuId integer not null,
	StatusId integer not null,
	foreign key(ParentId) references Orders(Id),
	foreign key(MenuId) references Menus(Id),
	foreign key(StatusId) references Statuses(Id)
);


-- insert default value
insert into Roles(Name) values('Admin');
insert into Roles(Name) values('Cashier');
insert into Roles(Name) values('Staff');
insert into Roles(Name) values('Guest');

insert into Categories(Name) values('Set');
insert into Categories(Name) values('Food');
insert into Categories(Name) values('Beverage');
insert into Categories(Name) values('Dessert');

insert into OrderTypes(Name) values('Dine in');
insert into OrderTypes(Name) values('Take out');

insert into Statuses(Name) values('Confirm');
insert into Statuses(Name) values('Complete');
