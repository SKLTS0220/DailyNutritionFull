# DailyNutrition 完整可編譯版本

執行： dotnet restore -> dotnet run
EXE： dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:SelfContained=true


🧩 專案本體（Razor Pages）

Program.cs（已設定 Razor Pages + SQLite）
Pages/

Index.cshtml：今天吃什麼（大字 UI）
History.cshtml：今天 / 昨天安心對比


Models/

FoodItem
DailyMeal
NutritionStatus


Data/

AppDbContext
SeedData（固定安全菜色）


README.md（操作與發佈說明）