param (
	[string]$migration_name = $(throw "Please specify a migration name.")
)

cd ../src/api/DND_5e.Api/
dotnet ef migrations add $migration_name --project ../DnD_5e.Infrastructure/DnD_5e.Infrastructure.csproj --context CharacterDbContext
dotnet ef database update --project ../DnD_5e.Infrastructure/DnD_5e.Infrastructure.csproj --context CharacterDbContext