-- This is SQLite build script
PRAGMA foreign_keys = ON;

--drop table OrderSubMenu;
--drop table SubMenu;
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
	Id integer NOT NULL PRIMARY KEY autoincrement,
	Name varchar(15) NOT NULL
);

CREATE TABLE IF NOT EXISTS User(
	Id integer NOT NULL PRIMARY KEY autoincrement,
	Username varchar(30) NOT NULL,
	Password text,
	RoleId integer NOT NULL,
	Displayname text,
	Street1 text,
	Street2 text,
	Postcode text,
	City text,
	State text,
	Country text,
	Point integer,
	FOREIGN KEY(RoleId) REFERENCES Role(Id)
);

CREATE TABLE IF NOT EXISTS Category(
	Id integer NOT NULL PRIMARY KEY autoincrement,
	Name varchar(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS Status(
	Id integer NOT NULL PRIMARY KEY autoincrement,
	Name varchar(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS Menu(
	Id	integer NOT NULL PRIMARY KEY autoincrement,
	CategoryId integer,
	Code text NOT NULL,
	Name text,
	Description text,
	Price real NOT NULL,
	FOREIGN KEY(CategoryId) REFERENCES Category(Id)
);

CREATE TABLE IF NOT EXISTS SubMenu(
	Id	integer NOT NULL PRIMARY KEY autoincrement,
	ParentId INTEGER NOT NULL,
	Name text,
	Price real NOT NULL,
	FOREIGN KEY(ParentId) REFERENCES Menu(Id)
);

-- stored charge percentage
CREATE TABLE IF NOT EXISTS Charge(
	Id integer NOT NULL PRIMARY KEY autoincrement,
	Name varchar(30) NOT NULL,
	Value real NOT NULL
);

CREATE TABLE IF NOT EXISTS OrderType(
	Id integer NOT NULL PRIMARY KEY autoincrement,
	Name varchar(30) NOT NULL
);

CREATE TABLE IF NOT EXISTS 'Order' (
	Id integer NOT NULL PRIMARY KEY autoincrement,
	CreatedById integer NOT NULL,
	Created datetime NOT NULL,
	DODate datetime,
	ReceiptDate datetime,
	QueueNo text,
	TableNo text,
	MemberId integer,
	Total real NOT NULL,
	FOREIGN KEY(CreatedById) REFERENCES User(Id)
);

CREATE TABLE IF NOT EXISTS OrderItem(
	Id integer NOT NULL PRIMARY KEY autoincrement,
	ParentId integer NOT NULL,
	OrderTypeId integer NOT NULL,
	MenuId integer NOT NULL,
	StatusId integer NOT NULL,
	FOREIGN KEY(ParentId) REFERENCES 'Order'(Id),
	FOREIGN KEY(OrderTypeId) REFERENCES OrderType(Id),
	FOREIGN KEY(MenuId) REFERENCES Menu(Id),
	FOREIGN KEY(StatusId) REFERENCES Status(Id)
);

CREATE TABLE IF NOT EXISTS OrderSubItem(
	Id integer NOT NULL PRIMARY KEY autoincrement,
	ParentId integer NOT NULL,
	SubMenuId integer NOT NULL,
	FOREIGN KEY(ParentId) REFERENCES OrderItem(Id),
	FOREIGN KEY(SubMenuId) REFERENCES SubMenu(Id)
);


-- insert default value
INSERT INTO Role(Name) VALUES('Admin');
INSERT INTO Role(Name) VALUES('Cashier');
INSERT INTO Role(Name) VALUES('Staff');
INSERT INTO Role(Name) VALUES('Guest');

INSERT INTO Category(Name) VALUES('Set Meal');
INSERT INTO Category(Name) VALUES('Food');
INSERT INTO Category(Name) VALUES('Beverage');
INSERT INTO Category(Name) VALUES('Dessert');

INSERT INTO OrderType(Name) VALUES('Dine in');
INSERT INTO OrderType(Name) VALUES('Take out');

INSERT INTO Status(Name) VALUES('Confirm');
INSERT INTO Status(Name) VALUES('Complete');
