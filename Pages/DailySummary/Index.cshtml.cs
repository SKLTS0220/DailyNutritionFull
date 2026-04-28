using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public class DailySummaryModel : PageModel
{
    private readonly AppDbContext _db;

    [BindProperty(SupportsGet = true)]
    public DateTime TargetDate { get; set; } = DateTime.Today;

    public DateTime WeekStart { get; set; }
    public DateTime WeekEnd { get; set; }

    // ✅ 一週所有餐點（含菜色）
    public List<DailyMeal> WeekMeals { get; set; } = new();

    public DailySummaryModel(AppDbContext db)
    {
        _db = db;
    }

    public void OnGet()
    {
        // 計算本週（星期一～星期日）
        int diff = (7 + (TargetDate.DayOfWeek - DayOfWeek.Monday)) % 7;
        WeekStart = TargetDate.AddDays(-diff).Date;
        WeekEnd = WeekStart.AddDays(6);

        // ✅ 一次把菜色 Join 進來
        WeekMeals = _db.DailyMeals
            .Where(m => m.Date >= WeekStart && m.Date <= WeekEnd)
            .Include(m => m.Items)
                .ThenInclude(i => i.FoodItem)
            .ToList();
    }

    // ✅ 直接回傳某天某餐的菜名字串
	public string GetFoodsText(DateTime date, MealType mealType)
	{
		var meal = WeekMeals.FirstOrDefault(m =>
			m.Date.Date == date.Date && m.MealType == mealType);

		if (meal == null || !meal.Items.Any())
			return "尚未";

		var mainFoods = meal.Items
			.Where(i => i.FoodItem.FoodType == FoodType.Main)
			.Select(i => i.FoodItem.Name)
			.OrderBy(n => n)
			.ToList();

		var sideFoods = meal.Items
			.Where(i => i.FoodItem.FoodType == FoodType.Side)
			.Select(i => i.FoodItem.Name)
			.OrderBy(n => n)
			.ToList();

		var lines = new List<string>();

		if (mainFoods.Any())
			lines.Add($"【主菜】{string.Join("、", mainFoods)}");

		if (sideFoods.Any())
			lines.Add($"【配菜】{string.Join("、", sideFoods)}");

		return string.Join("\n", lines);
	}
	
}