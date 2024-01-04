PS C:\repos\ChristmasTreeManager\src\ChristmasTreeManager> dotnet ef migrations add Initial --context ApplicationDbContext --project ../ChristmasTreeManager.Infrastructure.Sqlite --output-dir Migrations\Application -- --DatabaseProvider Sqlite

PS C:\repos\ChristmasTreeManager\src\ChristmasTreeManager> dotnet ef migrations add Initial --context IdentityDbContext --project ../ChristmasTreeManager.Infrastructure.Sqlite --output-dir Migrations\Identity -- --DatabaseProvider Sqlite

PS C:\repos\ChristmasTreeManager\src\ChristmasTreeManager> dotnet ef migrations add Initial --context IdentityDbContext --project ../ChristmasTreeManager.Infrastructure.Postgres --output-dir Migrations\Identity -- --DatabaseProvider Postgres

PS C:\repos\ChristmasTreeManager\src\ChristmasTreeManager> dotnet ef migrations add Initial --context ApplicationDbContext --project ../ChristmasTreeManager.Infrastructure.Postgres --output-dir Migrations\Application -- --DatabaseProvider Postgres