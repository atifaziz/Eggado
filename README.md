# Eggado

[![Build Status][win-build-badge]][win-builds]
[![Build Status][nix-build-badge]][nix-builds]
[![NuGet][nuget-badge]][nuget-pkg]
[![MyGet][myget-badge]][edge-pkgs]


C# and CLR have come a long way since their inception. ADO.NET has been around
as long but not evolved nearly as much. Eggado takes generics, lambdas,
expression trees, dynamic methods and DLR and uses them to breathe new life
into data access using good old ADO.NET. It's for folks who can live with a
SQL dialect.

It is not an ORM but take away the R or change it to mean _record_ and it does
the O and the M. In other words, it is more of an _object:record mapper_.

Eggado is independent of any ADO.NET provider. That's because it simply
extends and deals in terms of [`IDataRecord`][idrcd], [`IDataReader`][idrdr]
and [`IDbCommand`][idcmd].

Starting with version 2.2, the API has full asynchronous support
(`async`/`await`), including reading data as [asynchronous streams].

## Example 1

```c#
using (var connection = new SqlCeConnection(@"Data Source=C:\db\Northwind.sdf"))
using (var command = new SqlCeCommand(@"
    SELECT
        [Product ID] ProductId,
        [Product Name] ProductName,
        [Quantity Per Unit] QuantityPerUnit,
        [Unit Price] UnitPrice
    FROM
        Products",
    connection))
{
    connection.Open();

    var products = command.Select(
    (
        int productId, string productName,
        string quantityPerUnit, decimal unitPrice
    )
    => new
    {
        Id              = productId,
        Name            = productName,
        QuantityPerUnit = quantityPerUnit,
        UnitPrice       = unitPrice,
    });

    foreach (var product in products)
        Console.WriteLine(product);
}
```

## Example 2

```c#
using (var connection = new SqlCeConnection(@"Data Source=C:\db\Northwind.sdf"))
using (var command = new SqlCeCommand(@"
    SELECT
        P.[Product ID] ProductId,
        P.[Product Name] ProductName,
        P.[English Name] EnglishName,
        P.[Quantity Per Unit] QuantityPerUnit,
        P.[Unit Price] UnitPrice,
        P.[Units In Stock] UnitsInStock,
        P.[Units On Order] UnitsOnOrder,
        P.[Reorder Level] ReorderLevel,
        P.[Discontinued],
        S.[Company Name] Supplier,
        C.[Category Name] Category
    FROM
        Products P
    INNER JOIN Suppliers  S ON P.[Supplier ID] = S.[Supplier ID]
    INNER JOIN Categories C ON P.[Category ID] = C.[Category ID]",
    connection))
{
    connection.Open();

    var products = command.Select(
    (
        int productId, string productName, string englishName,
        string quantityPerUnit, decimal unitPrice,
        int unitsInStock, int unitsOnOrder, int reorderLevel,
        bool discontinued, string supplier, string category
    )
    => new
    {
        Id              = productId,
        Name            = productName,
        EnglishName     = englishName,
        QuantityPerUnit = quantityPerUnit,
        UnitPrice       = unitPrice,
        UnitsInStock    = unitsInStock,
        UnitsOnOrder    = unitsOnOrder,
        ReorderLevel    = reorderLevel,
        IsDiscontinued  = discontinued,
        Supplier        = supplier,
        Category        = category,
    });

    foreach (var product in products)
        Console.WriteLine(product);
}
```


[win-build-badge]: https://img.shields.io/appveyor/ci/raboof/eggado/master.svg?label=windows
[win-builds]: https://ci.appveyor.com/project/raboof/eggado
[nix-build-badge]: https://img.shields.io/travis/atifaziz/Eggado/master.svg?label=linux
[nix-builds]: https://travis-ci.org/atifaziz/Eggado
[myget-badge]: https://img.shields.io/myget/raboof/vpre/Eggado.svg?label=myget
[edge-pkgs]: https://www.myget.org/feed/raboof/package/nuget/Eggado
[nuget-badge]: https://img.shields.io/nuget/v/Eggado.svg
[nuget-pkg]: https://www.nuget.org/packages/Eggado

[idrcd]: https://msdn.microsoft.com/en-us/library/system.data.idatarecord.aspx
[idrdr]: https://msdn.microsoft.com/en-us/library/system.data.idatareader.aspx
[idcmd]: https://msdn.microsoft.com/en-us/library/system.data.idbcommand.aspx
[asynchronous streams]: https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/generate-consume-asynchronous-stream
