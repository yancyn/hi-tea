-- This is SQLite build script
PRAGMA foreign_keys = ON;

--drop table OrderItem;
--drop table 'Order';
--drop table OrderType;
--drop table Charge;
--drop table Menu;
--drop table Status;
--drop table Category;
--drop table User;
--drop table Role;

CREATE TABLE IF NOT EXISTS Role(
	Id integer NOT NULL primary key autoincrement,
	Name varchar(15)
);

CREATE TABLE IF NOT EXISTS User(
	Id integer NOT NULL primary key autoincrement,
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
	foreign key(RoleId) references Role(Id)
);

CREATE TABLE IF NOT EXISTS Category(
	Id integer NOT NULL primary key autoincrement,
	Name varchar(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS Status(
	Id integer NOT NULL primary key autoincrement,
	Name varchar(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS Menu(
	Id	integer NOT NULL primary key autoincrement,
	CategoryId integer,
	Code text NOT NULL,
	Name text,
	Price real NOT NULL,
	foreign key(CategoryId) references Category(Id)
);

-- stored charge percentage
CREATE TABLE IF NOT EXISTS Charge(
	Id integer NOT NULL primary key autoincrement,
	Name varchar(30),
	Value real NOT NULL
);

CREATE TABLE IF NOT EXISTS OrderType(
	Id integer NOT NULL primary key autoincrement,
	Name varchar(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS 'Order' (
	Id integer NOT NULL primary key autoincrement,
	CreatedById integer NOT NULL,
	Created datetime NOT NULL,
	DODate datetime,
	ReceiptDate datetime,
	TableNo text,
	MemberId integer,
	Total real NOT NULL,
	foreign key(CreatedById) references User(Id)
);

CREATE TABLE IF NOT EXISTS OrderItem(
	Id integer NOT NULL primary key autoincrement,
	ParentId integer NOT NULL,
	MenuId integer NOT NULL,
	StatusId integer NOT NULL,
	foreign key(ParentId) references 'Order'(Id),
	foreign key(MenuId) references Menu(Id),
	foreign key(StatusId) references Status(Id)
);


-- insert default value
insert into Role(Name) values('Admin');
insert into Role(Name) values('Cashier');
insert into Role(Name) values('Staff');
insert into Role(Name) values('Guest');

insert into Category(Name) values('Set Meal');
insert into Category(Name) values('Food');
insert into Category(Name) values('Beverage');
insert into Category(Name) values('Dessert');

insert into OrderType(Name) values('Dine in');
insert into OrderType(Name) values('Take out');

insert into Status(Name) values('Confirm');
insert into Status(Name) values('Complete');
