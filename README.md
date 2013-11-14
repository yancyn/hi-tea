hi-tea
======
A POS for café.

Generate dbml from DbLinq
=========================

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

Hardwares
=========
1. Set default printer to use 'Cashier'.
2. Label printer as 'Bar'.


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
