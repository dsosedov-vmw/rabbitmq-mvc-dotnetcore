dotnet publish src -c Release -f netcoreapp3.1 --self-contained -r linux-x64 -o src\Web\bin\Release\publish
cf push
