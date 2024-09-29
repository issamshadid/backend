#
# EfAddMigrationStep.ps1
#
param(
    [string]$migrationName,
    [string]$dbContext
)

if ( [string]::IsNullOrEmpty($dbContext))
{
    $dbContext = "AppDbContext";
}

dotnet ef migrations add -c $dbContext $migrationName -v
