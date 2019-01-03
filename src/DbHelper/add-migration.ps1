param([string]$n)

dotnet ef --project ..\DbService\DbService.csproj migrations add $n