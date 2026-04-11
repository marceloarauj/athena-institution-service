@echo off
dotnet ef database update ^
  --project Institution.Infrastructure\Institution.Infrastructure.csproj ^
  --startup-project Institution.Infrastructure\Institution.Infrastructure.csproj