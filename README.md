# SimpleMysql
Simplify MySQL implementation


## Getting Started

```
Mysql mysql = new Mysql("127.0.0.1", "database", "admin", "admin");
DataTable result = mysql.Query("SELECT * FROM database.table;");
foreach (DataRow row in result.Rows)
{
	Console.WriteLine(string.Join("|", row.ItemArray));
}
mysql.Dispose();
```


### Prerequisities

.NET Framework 4


### Installing

https://www.nuget.org/packages/SimpleMysql/


## Built With

visual studio 2013


## License

https://github.com/herbertagosto/SimpleMysql/blob/master/LICENSE_3RD_PARTY
https://github.com/herbertagosto/SimpleMysql/blob/master/LICENSE

