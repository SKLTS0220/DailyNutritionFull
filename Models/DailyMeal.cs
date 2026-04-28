using System;
using System.Collections.Generic;

public enum MealType
{
    Breakfast = 0,
    Lunch = 1,
    Dinner = 2,
    MidnightSnack = 3   // ✅ 新增：宵夜
}

public enum NutritionStatus
{
    Safe,
    Warning
}

public class DailyMeal
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public MealType MealType { get; set; }

    public NutritionStatus Status { get; set; }

    // ✅ 一餐實際選的菜色
    public List<DailyMealItem> Items { get; set; } = new();
}
