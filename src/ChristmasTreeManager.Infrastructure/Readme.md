...\src\ChristmasTreeManager> dotnet ef migrations add Initial --context ApplicationDbContext --project ../ChristmasTreeManager.Infrastructure.Sqlite --output-dir Migrations\Application -- --DatabaseProvider Sqlite

...\src\ChristmasTreeManager> dotnet ef migrations add Initial --context IdentityDbContext --project ../ChristmasTreeManager.Infrastructure.Sqlite --output-dir Migrations\Identity -- --DatabaseProvider Sqlite


...\src\ChristmasTreeManager> dotnet ef migrations add Initial --context IdentityDbContext --project ../ChristmasTreeManager.Infrastructure.MySql --output-dir Migrations\Identity -- --DatabaseProvider MySql

...\src\ChristmasTreeManager> dotnet ef migrations add Initial --context ApplicationDbContext --project ../ChristmasTreeManager.Infrastructure.MySql --output-dir Migrations\Application -- --DatabaseProvider MySql


...\src\ChristmasTreeManager> dotnet ef migrations add Initial --context IdentityDbContext --project ../ChristmasTreeManager.Infrastructure.Postgres --output-dir Migrations\Identity -- --DatabaseProvider Postgres

...\src\ChristmasTreeManager> dotnet ef migrations add Initial --context ApplicationDbContext --project ../ChristmasTreeManager.Infrastructure.Postgres --output-dir Migrations\Application -- --DatabaseProvider Postgres
