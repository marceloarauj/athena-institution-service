@echo off
dotnet ef migrations add %1 --project Institution.Infrastructure --startup-project Institution.Infrastructure