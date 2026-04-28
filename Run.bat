cd C:\Users\T112606\source\DailyNutritionFull

dotnet clean
rem dotnet restore --no-cache
rem dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:SelfContained=true --no-restore

dotnet run

pause