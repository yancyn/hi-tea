hi-tea
======
A WPF POS application for café which is based on SQLite as database and aims for touchscreen device.

How to Run
========
1. Download latest copy from deploy folder - https://github.com/yancyn/hi-tea/tree/master/deploy.
2. Download database - pos.db3 at deploy folder.
3. Unzip the zip file and save as your local folder.
4. Modify connection string in PosWPF.exe.config to your pos.db3 location. ie. E:\yourfolder\pos.db3.
5. Start PosWPF.exe.

Connection String
-------------------
    <connectionStrings>
        <add name="PosConnectionString" connectionString="DbLinqProvider=Sqlite;DbLinqConnectionType=System.Data.SQLite.SQLiteConnection, System.Data.SQLite, Version=1.0.66.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139;Data Source=E:\projects\hi-tea\src\PosMvc\App_Data\pos.db3;" providerName="SQLite"/>
    </connectionStrings>


Generate dbml from DbLinq
======================

	DbMetal /provider:Sqlite /conn "Data Source=File.db3" /dbml:File.dbml
	DbMetal /code:File.cs File.dbml

Setup IIS Express
=================
To allow everyone can accecc your local IP.

    > netsh http add urlacl url=http://192.168.1.11:1234/ user=everyone
    
Method 1 (Preferable)
-----------------------
Through applicationhost.config

    "C:\Program Files\IIS Express\iisexpress.exe" /config:d:\HiTea\applicationhost.config /site:HiTea


Method 2
----------
Default page is index.html

	iisexpress /path:d:\HiTea\ /port:9090

Useful SQLite Commands
=================
1. Backup database

        sqlite3 pos.db3 .dump > pos.bak
    
2. Daily sales

        SELECT strftime('%Y-%m-%d',Created) AS Date, COUNT(Id) AS Orders, SUM(Total) AS Sales FROM 'Order' GROUP BY strftime('%Y-%m-%d',Created);

Hardwares
=========
1. Set default printer to use 'Cashier'.
2. Label printer as 'Bar'.
3. Tablet size landscape 448x1067px, portrait 915x640px.

Adb Commands
===========
    adb push pos.db3 /sdcard/pos.db3

References
=======
- [TVS Touch Screen Monitor](http://www.tvs.com.tw/)
- [OPOS](http://en.wikipedia.org/wiki/OPOS)
- [SQLite](http://www.sqlite.org/)
- [ADO.NET 2.0 Provider for SQLite](http://sourceforge.net/projects/sqlite-dotnet2/)
- [DbLinq](http://code.google.com/p/dblinq2007/)
- [Metro Style](http://mahapps.com/MahApps.Metro/) [source](https://github.com/MahApps/MahApps.Metro) require .NET 4.0 and above
- [Borderless Window](https://wpfborderless.codeplex.com/)
- [Escalier Theme](http://www.freecsstemplates.org/preview/escalier/)
- [WPF MessageBox](https://msgbox.codeplex.com/)
- [Check Browser Size](http://resizemybrowser.com/)