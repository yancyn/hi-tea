-- This is SQLite build script
PRAGMA foreign_keys = ON;

--drop table OrderSubItem;
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
	Email text,
	IC text,
	Mobile text,
	Point integer NOT NULL,
	Active bit NOT NULL DEFAULT 1,
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
	CategoryId integer NOT NULL,
	Code text NOT NULL,
	Name text,
	Description text,
	Image text,
	Price real NOT NULL,
	Active bit NOT NULL DEFAULT 1,
	FOREIGN KEY(CategoryId) REFERENCES Category(Id)
);

-- stored charge percentage
CREATE TABLE IF NOT EXISTS Charge(
	Id integer NOT NULL PRIMARY KEY autoincrement,
	Name varchar(30) NOT NULL,
	Value real NOT NULL,
	Active bit NOT NULL DEFAULT 1
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
	MemberId integer NOT NULL DEFAULT 0,
	Total real NOT NULL,
	FOREIGN KEY(MemberId) REFERENCES User(Id)
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
	MenuId integer NOT NULL,
	FOREIGN KEY(ParentId) REFERENCES OrderItem(Id),
	FOREIGN KEY(MenuId) REFERENCES Menu(Id)
);


-- insert default value
INSERT INTO Role(Name) VALUES('Admin');
INSERT INTO Role(Name) VALUES('Cashier');
INSERT INTO Role(Name) VALUES('Staff');
INSERT INTO Role(Name) VALUES('Guest');

INSERT INTO Category(Name) VALUES('Addon');
INSERT INTO Category(Name) VALUES('Set Menu');
INSERT INTO Category(Name) VALUES('Food');
INSERT INTO Category(Name) VALUES('Snack');
INSERT INTO Category(Name) VALUES('Drink');
INSERT INTO Category(Name) VALUES('Dessert');

INSERT INTO OrderType(Name) VALUES('Dine in');
INSERT INTO OrderType(Name) VALUES('Take out');

INSERT INTO Status(Name) VALUES('Confirm');
INSERT INTO Status(Name) VALUES('Complete');


--Add User
insert into User(Username,Password,RoleId,DisplayName,Point,Active) values('admin','admin',1,'admin',0,1);
insert into User(Username,Password,RoleId,DisplayName,Point,Active) values('cashier','cashier',2,'cashier',0,1);

-- Add charges
insert into Charge(Name,Value,Active) values('Govn Tax 6%',0.06,1);
insert into Charge(Name,Value,Active) values('Member Discount',-0.1,1);

--Add menu
-------------------------------------------
-- Set Menu
-------------------------------------------
INSERT INTO "Menu" VALUES(1,2,'S01','套餐 Set 1',NULL,'S01.jpg',8.9,1);
INSERT INTO "Menu" VALUES(2,2,'S02','套餐 Set 2',NULL,'S02.jpg',7.9,1);
INSERT INTO "Menu" VALUES(3,2,'S03','套餐 Set 3',NULL,'S03.jpg',6.9,1);
INSERT INTO "Menu" VALUES(4,2,'S04','套餐 Set 4',NULL,'S04.jpg',7.9,1);
INSERT INTO "Menu" VALUES(5,2,'S05','套餐 Set 5',NULL,'S05.jpg',8.9,1);
INSERT INTO "Menu" VALUES(6,2,'S06','套餐 Set 6',NULL,'S06.jpg',5.5,1);
INSERT INTO "Menu" VALUES(7,2,'S07','套餐 Set 7',NULL,'S07.jpg',7.9,1);
INSERT INTO "Menu" VALUES(8,2,'S08','套餐 Set 8',NULL,'S08.jpg',7.9,1);

-------------------------------------------
-- Food
-------------------------------------------
INSERT INTO "Menu" VALUES(9,3,'B01','黑椒鸡扒 Black Pepper Chicken Chop',NULL,'B01.jpg',11.9,1);
INSERT INTO "Menu" VALUES(10,3,'B02','黑椒芝士焗饭 Black Pepper Cheese Baked Rice',NULL,'B02.jpg',11.9,1);
INSERT INTO "Menu" VALUES(11,3,'B03','创新意大利面 Itali Pasta',NULL,'B03.jpg',9.9,1);
INSERT INTO "Menu" VALUES(12,3,'B04','法式芝士焗意大利面 French Cheese Baked Itali Pasta',NULL,'B04.jpg',9.9,1);
INSERT INTO "Menu" VALUES(13,3,'B05','芝士培根意大利面 Cheese Bacon Pasta',NULL,'B05.jpg',8.9,1);
INSERT INTO "Menu" VALUES(14,3,'B06','马六甲椰浆鸡饭 Nasi Lemak Ayam Merah',NULL,'B06.jpg',7.9,1);
INSERT INTO "Menu" VALUES(15,3,'B07','马六甲三拜虾饭 Malacca Sambl Prawn Rice',NULL,'B07.jpg',6.9,1);
INSERT INTO "Menu" VALUES(16,3,'B08','梹城马铃薯肉丸饭 Penang Potato Meat Ball Rice',NULL,'B08.jpg',5.9,1);
INSERT INTO "Menu" VALUES(17,3,'B09','家乡炒饭 Home Town Fried Rice',NULL,'B09.jpg',5.9,1);
INSERT INTO "Menu" VALUES(18,3,'B10','韩国泡菜炒饭 Korea Kimchi Fried Rice',NULL,'B10.jpg',6.9,1);
INSERT INTO "Menu" VALUES(19,3,'B11','韩国奶汁三文鱼炒饭 Korea Milk Salmon Fried Rice',NULL,'B11.jpg',7.9,1);
INSERT INTO "Menu" VALUES(20,3,'B12','泰式炒饭 Thai Frid Rice',NULL,'B12.jpg',5.9,1);
INSERT INTO "Menu" VALUES(21,3,'B13','铜锣湾砂煲鸡饭 Causeway Bay Clay Pot Chicken Rice',NULL,'B13.jpg',6.9,1);
INSERT INTO "Menu" VALUES(22,3,'B14','啤酒鸡饭 Beer Chicken Rice',NULL,'B14.jpg',6.9,1);
INSERT INTO "Menu" VALUES(23,3,'B15','上海叉烧鸡饭 Shanghai BBQ Chicken Rice',NULL,'B15.jpg',7.9,1);
INSERT INTO "Menu" VALUES(24,3,'B16','北京酸甜鱼片饭 Beijing Sweet & Sour Fish Meal',NULL,'B16.jpg',6.9,1);
INSERT INTO "Menu" VALUES(25,3,'B17','新加坡云吞面 Singapore Wantan Noodle',NULL,'B17.jpg',4.5,1);
INSERT INTO "Menu" VALUES(26,3,'B18','新加坡煎虾云吞面 Singapore Wantan Noodle with Fried Prawn',NULL,'B18.jpg',6.9,1);

-------------------------------------------
-- Snack
-------------------------------------------
INSERT INTO "Menu" VALUES(27,4,'C01','芝士番茄烤面包 Tomato Cheese Baked Toast',NULL,'C01.jpg',6.5,1);
INSERT INTO "Menu" VALUES(28,4,'C02','鸡蛋烤面包 Egg Baked Toast',NULL,'C02.jpg',5.9,1);
INSERT INTO "Menu" VALUES(29,4,'C03','芝士培根烤面包 Cheese Bacon Baked Toast',NULL,'C03.jpg',6.5,1);
INSERT INTO "Menu" VALUES(30,4,'C04','唤醒身体面包 Shake Your Body Bread',NULL,'C04.jpg',7.9,1);
INSERT INTO "Menu" VALUES(31,4,'C05','三文治 Tuna Sandwich',NULL,'C05.jpg',5.9,1);
INSERT INTO "Menu" VALUES(32,4,'C06','法式面包蘑菇汤 French Bread & Mushroom Soup',NULL,'C06.jpg',5.9,1);
INSERT INTO "Menu" VALUES(33,4,'C07','肉松美乃虾 Meat Floss Mayonaise Prawns',NULL,'C07.jpg',7.9,1);
INSERT INTO "Menu" VALUES(34,4,'C08','土豆培根焗芝士 Potato Bacon Baked Cheese',NULL,'C08.jpg',6.5,1);
INSERT INTO "Menu" VALUES(35,4,'C09','南乳鸡翅膀 Bean Curl Chicken Wings',NULL,'C09.jpg',8.9,1);
INSERT INTO "Menu" VALUES(36,4,'C10','曼谷东炎鸡翅膀 Bangkok Tomyam Chicken Wings',NULL,'C10.jpg',8.9,1);
INSERT INTO "Menu" VALUES(37,4,'C11','香炸甜不辣 Fried Tempura 6''s',NULL,'C11.jpg',4.9,1);
INSERT INTO "Menu" VALUES(38,4,'C12','西湾串烧三文鱼 Saiwan Salmon Skewers',NULL,'C12.jpg',6.9,0);
INSERT INTO "Menu" VALUES(39,4,'C13','上海虾仁培根卷 Shanghai Shrimp & Bacon Roll',NULL,'C13.jpg',9.9,1);
INSERT INTO "Menu" VALUES(40,4,'C14','上海芝士培根卷 Shanghai Cheese Bacon Roll',NULL,'C14.jpg',8.9,1);
INSERT INTO "Menu" VALUES(41,4,'C15','炸薯条 French Fries',NULL,'C15.jpg',4.9,1);

-------------------------------------------
-- Drink
-------------------------------------------
INSERT INTO "Menu" VALUES(42,5,'MS01','白糖招牌原汁豆浆 Special Original Soy Milk',NULL,'MS01.jpg',2.5,1);
INSERT INTO "Menu" VALUES(43,5,'MS02','黑糖招牌原汁豆浆 Special Soy Milk(Brown Sugar)',NULL,'MS02.jpg',2.5,1);
INSERT INTO "Menu" VALUES(44,5,'MS03','黑糖水晶招牌豆浆 Special Soy Milk(Brown Sugar) with Black Crystal',NULL,'MS03.jpg',3.8,1);
INSERT INTO "Menu" VALUES(45,5,'MS04','招牌原汁豆浆加三色寒天 Special Soy Milk with 3 Colors',NULL,'MS04.jpg',3.8,1);
INSERT INTO "Menu" VALUES(46,5,'MS05','白糖水晶招牌原汁豆浆 Special Soy Milk with Black Crystal',NULL,'MS05.jpg',3.8,1);
INSERT INTO "Menu" VALUES(47,5,'MS06','芋香豆浆 Soy Milk with Yam',NULL,'MS06.jpg',3.8,1);
INSERT INTO "Menu" VALUES(48,5,'MS07','草莓豆浆 Soy Milk with Strawberry',NULL,'MS07.jpg',3.8,1);
INSERT INTO "Menu" VALUES(49,5,'MS08','巧克力豆浆 Soy Milk with Chocolate',NULL,'MS08.jpg',3.8,1);
INSERT INTO "Menu" VALUES(50,5,'MS09','香草豆浆 Soy Milk with Vanilla',NULL,'MS09.jpg',3.8,1);
INSERT INTO "Menu" VALUES(51,5,'MS10','抺茶豆浆 Soy Milk with Matcha',NULL,'MS10.jpg',3.8,1);
INSERT INTO "Menu" VALUES(52,5,'MS11','芒果豆浆 Soy Milk with Mango',NULL,'MS11.jpg',3.8,1);
INSERT INTO "Menu" VALUES(53,5,'MS12','薄荷豆浆 Soy Milk with Mint',NULL,'MS12.jpg',3.8,1);
INSERT INTO "Menu" VALUES(54,5,'MS13','哈密瓜豆浆 Soy Milk with Honeydew',NULL,'MS13.jpg',3.8,1);

INSERT INTO "Menu" VALUES(55,5,'CSS01','招牌咖啡豆奶 Signature Soy Milk Coffee',NULL,'SS01.jpg',5,1);
INSERT INTO "Menu" VALUES(56,5,'CSS02','焦糖咖啡 Caramel Soy Milk Coffee',NULL,'SS02.jpg',5,1);
INSERT INTO "Menu" VALUES(57,5,'CSS03','榛子咖啡 Hazelnut Soy Milk Coffee',NULL,'SS03.jpg',5,1);
INSERT INTO "Menu" VALUES(58,5,'CSS04','香草咖啡 Vanilla Soy Milk Coffee',NULL,'SS04.jpg',5,1);
INSERT INTO "Menu" VALUES(59,5,'CSS05','北海道咖啡 Hakaido Soy Milk Coffee',NULL,'SS05.jpg',5,1);
INSERT INTO "Menu" VALUES(60,5,'CSS06','卡布奇诺 Soy Milk Cappucino',NULL,'SS06.jpg',5,1);
INSERT INTO "Menu" VALUES(61,5,'CSS07','摩卡 Soy Milk Mocha',NULL,'SS07.jpg',5,1);
INSERT INTO "Menu" VALUES(62,5,'CSS08','拿铁 Soy Milk Latte',NULL,'SS08.jpg',5,1);
INSERT INTO "Menu" VALUES(63,5,'CSS09','提拉米苏 Soy Milk Tiramisu',NULL,'SS09.jpg',5,1);

INSERT INTO "Menu" VALUES(64,5,'MT01','招牌豆奶茶 Signature Soy Milk Tea',NULL,'MT01.jpg',4.5,1);
INSERT INTO "Menu" VALUES(65,5,'MT02','焦糖豆奶茶 Caramel Soy Milk Tea',NULL,'MT02.jpg',4.8,1);
INSERT INTO "Menu" VALUES(66,5,'MT03','榛子豆奶茶 Hazelnut Soy Milk Tea',NULL,'MT03.jpg',4.8,1);
INSERT INTO "Menu" VALUES(67,5,'MT04','香草豆奶茶 Vanilla Soy Milk Tea',NULL,'MT04.jpg',4.8,1);
INSERT INTO "Menu" VALUES(68,5,'MT05','北海道豆奶茶 Hakaido Soy Milk Tea',NULL,'MT05.jpg',4.8,1);

INSERT INTO "Menu" VALUES(69,5,'FT01','冬瓜 Wintermelon',NULL,'FT01.jpg',4,1);
INSERT INTO "Menu" VALUES(70,5,'FT02','芒果 Mango',NULL,'FT02.jpg',4,1);
INSERT INTO "Menu" VALUES(71,5,'FT03','百香果 Passion Fruit',NULL,'FT03.jpg',4,1);
INSERT INTO "Menu" VALUES(72,5,'FT04','水蜜桃 Peach',NULL,'FT04.jpg',4,1);
INSERT INTO "Menu" VALUES(73,5,'FT05','柠檬 Lemon',NULL,'FT05.jpg',4,1);
INSERT INTO "Menu" VALUES(74,5,'FT06','蜂蜜 Honey',NULL,'FT06.jpg',4,1);
INSERT INTO "Menu" VALUES(75,5,'FT07','蜂蜜柠檬 Honey Lemon',NULL,'FT07.jpg',4,1);

-------------------------------------------
-- Dessert
-------------------------------------------
INSERT INTO "Menu" VALUES(76,6,'E01','招牌白糖豆腐花 Tofu Pudding White Sugar',NULL,'E01.jpg',2.8,1);
INSERT INTO "Menu" VALUES(77,6,'E02','招牌黑糖豆腐花 Tofu Pudding Brown Sugar',NULL,'E02.jpg',2.8,1);
INSERT INTO "Menu" VALUES(78,6,'E03','火龙果豆腐花 Dragon Fruit Tofu Pudding',NULL,'E03.jpg',4.3,1);
INSERT INTO "Menu" VALUES(79,6,'E04','水晶冷豆腐花 Crystal Tofu Pudding',NULL,'E04.jpg',4.3,1);
INSERT INTO "Menu" VALUES(80,6,'E05','红豆冷豆花 Red Bean Tofu Pudding',NULL,'E05.jpg',4.3,1);
INSERT INTO "Menu" VALUES(81,6,'E06','绿豆冷豆花 Green Bean Tofu Pudding',NULL,'E06.jpg',4.3,1);
INSERT INTO "Menu" VALUES(82,6,'E07','地瓜冷豆花 Crystal Tofu Pudding',NULL,'E07.jpg',4.3,1);
INSERT INTO "Menu" VALUES(83,6,'E08','特调豆花 Special Tofu Pudding',NULL,'E08.jpg',5.3,1);
INSERT INTO "Menu" VALUES(84,6,'E09','草莓豆水雪花冰 Strawberry Soya Ice Corn Flick',NULL,'E09.jpg',7.5,1);
INSERT INTO "Menu" VALUES(85,6,'E10','招牌豆水雪花冰 Soya Ice Corn Flick',NULL,'E10.jpg',7.5,1);
INSERT INTO "Menu" VALUES(86,6,'E11','芒果味雪花冰 Mango Ice Corn Flick',NULL,'E11.jpg',7.5,1);

INSERT INTO "Menu" VALUES(87,7,'SD01','豆豆甜点 Kangen Fresh Signature',NULL,'SD01.jpg',6.9,1);
INSERT INTO "Menu" VALUES(88,7,'SD02','八仙过海 Eight Immortials',NULL,'SD02.jpg',6.9,1);
INSERT INTO "Menu" VALUES(89,7,'SD03','百年好合 Love For All Seasons',NULL,'SD03.jpg',6.9,1);

INSERT INTO "Menu" VALUES(90,7,'F01','魔蝎座芋香 Capricon - Yam',NULL,'F01.jpg',5.5,1);
INSERT INTO "Menu" VALUES(100,7,'F02','水瓶座草莓 Aquarius - Strawberry',NULL,'F02.jpg',5.5,1);
INSERT INTO "Menu" VALUES(101,7,'F03','双鱼座巧克力 Pisces - Chocolate',NULL,'F03.jpg',5.5,1);
INSERT INTO "Menu" VALUES(102,7,'F04','白羊座香草 Aries - Vanilla',NULL,'F04.jpg',5.5,1);
INSERT INTO "Menu" VALUES(103,7,'F05','金牛座抺茶 Taurus - Matcha',NULL,'F05.jpg',5.5,1);
INSERT INTO "Menu" VALUES(104,7,'F06','双子座芒果 Gemini - Mango',NULL,'F06.jpg',5.5,1);
INSERT INTO "Menu" VALUES(105,7,'F07','巨蟹座薄荷 Cancer - Mint',NULL,'F07.jpg',5.5,1);
INSERT INTO "Menu" VALUES(106,7,'F08','狮子座红豆 Leo - Red Bean',NULL,'F08.jpg',5.5,1);
INSERT INTO "Menu" VALUES(107,7,'F09','处女座摩卡 Virgo - Mocha',NULL,'F09.jpg',5.5,1);
INSERT INTO "Menu" VALUES(108,7,'F10','天秤座水蜜桃 Libra - Peach',NULL,'F10.jpg',5.5,1);
INSERT INTO "Menu" VALUES(109,7,'F11','天蝎座百香果 Scorpio - Passion Fruit',NULL,'F11.jpg',5.5,1);
INSERT INTO "Menu" VALUES(110,7,'F12','射手座哈密瓜 Sagittarius - Honey Dew',NULL,'F12.jpg',5.5,1);

-------------------------------------------
-- Add on
-------------------------------------------
INSERT INTO "Menu" VALUES(111,1,'A01','加蛋 Egg',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(112,1,'A02','加饭 Rice',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(113,1,'A03','加米粉 Beehun',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(114,1,'A01','加面 Mee',NULL,NULL,1,1);

INSERT INTO "Menu" VALUES(115,1,'T01','红豆 Red Bean',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(116,1,'T02','绿豆 Green Bean',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(117,1,'T03','黑糖水晶 Brown Cyrstal',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(118,1,'T04','原味水晶 Original Cyrstal',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(119,1,'T05','珍珠 Pearl',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(120,1,'T06','地瓜 Sweet Potatoes',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(121,1,'T06','芦荟 Aloe Vera',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(122,1,'T07','三色寒天 QQ Jerry',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(123,1,'T08','Nata De Coco',NULL,NULL,1,1);
INSERT INTO "Menu" VALUES(124,1,'T09','仙草 Glass Jerry',NULL,NULL,1,1);
