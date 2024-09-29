#
# Update_Database.ps1
#
param(
    [string]$mode,
    [string]$connectionString
)

if ( [string]::IsNullOrEmpty($mode))
{
    $mode = "dev";
}
if ( [string]::IsNullOrEmpty($connectionString))
{
    $connectionString = "Server=.;Initial Catalog=TEMPLATE;Persist Security Info=False;User ID=sa;Password=v2hcD645;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=300;"
}

# In 'dev' mode use a different app path
$appPath = ".\Template.Migrations.dll";
if ($mode -eq "dev")
{
    $appPath = "./bin/Debug/net8.0/Template.Migrations.dll";
}

# Execute
dotnet $appPath $connectionString