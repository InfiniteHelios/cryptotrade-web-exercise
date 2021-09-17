# cryptotrade-Blazor
This is Cryptocurrency Website developed with Blazor

## Installation
This requires dotnet.

Install dotnet-ef tool.
```sh
cd WebApplication2
dotnet tool install --global dotnet-ef
```

Make migraion files.
```sh
dotnet ef migrations add Init
```

Migrate to MySQL database.
```sh
dotnet ef database update
```

Run
```sh
dotnet run
```

For development...
```sh
dotnet watch run
```
