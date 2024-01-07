...\src\ChristmasTreeManager> dotnet ef migrations add AddTeamLeader --context ApplicationDbContext --project ../ChristmasTreeManager.Infrastructure.Sqlite --output-dir Migrations\Application -- --DatabaseProvider Sqlite

...\src\ChristmasTreeManager> dotnet ef migrations add AddTeamLeader --context ApplicationDbContext --project ../ChristmasTreeManager.Infrastructure.Postgres --output-dir Migrations\Application -- --DatabaseProvider Postgres

...\src\ChristmasTreeManager> dotnet ef migrations add AddTeamLeader --context IdentityDbContext --project ../ChristmasTreeManager.Infrastructure.MySql --output-dir Migrations\Identity -- --DatabaseProvider MySql

...\src\ChristmasTreeManager> dotnet ef migrations add AddTeamLeader --context ApplicationDbContext --project ../ChristmasTreeManager.Infrastructure.MySql --output-dir Migrations\Application -- --DatabaseProvider MySql
