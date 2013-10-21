-- This is SQLite build script
PRAGMA foreign_keys = ON;

create table if not exists roles(
	role varchar(15) primary key
);

create table if not exists users(
	username varchar(30) primary key,
	password text,
	role varchar(15),
	displayname text,
	street1 text,
	street2 text,
	postcode text,
	city text,
	state text,
	country text,
	point integer,
	foreign key(role) references roles(role)
);

create table if not exists categories(
	category varchar(30) primary key
);

create table if not exists statuses(
	status varchar(30) primary key
);

create table if not exists menus(
	id	integer primary key autoincrement,
	category varchar(30),
	code text,
	name text,
	price real,
	foreign key(category) references categories(category)
);

-- stored charge percentage
create table if not exists charges(
	charge varchar(30) primary key,
	value real
);

create table if not exists ordertypes(
	type varchar(30) primary key
);

create table if not exists orders(
	id integer primary key autoincrement,
	created datetime,
	tableno smallint,
	member varchar(30),
	total real,
	foreign key(member) references users(username)
);
create table if not exists orderitems(
	id integer primary key autoincrement,
	parent integer not null,
	menu integer not null,
	status varchar(30) not null,
	foreign key(parent) references orders(id),
	foreign key(menu) references menus(id),
	foreign key(status) references statuses(status)
);
create table if not exists ordercharges(
	id integer primary key autoincrement,
	charge varchar(30) not null,
	parent integer not null,
	foreign key(charge) references charges(charge),
	foreign key(parent) references orders(id)
);


-- insert default value
insert into roles(role) values('admin');
insert into roles(role) values('cashier');
insert into roles(role) values('staff');
insert into roles(role) values('customer');

insert into categories(category) values('set');
insert into categories(category) values('food');
insert into categories(category) values('drink');
insert into categories(category) values('dessert');

insert into ordertypes(type) values('dine in');
insert into ordertypes(type) values('take out');

insert into statuses(status) values('confirm');
insert into statuses(status) values('complete');
