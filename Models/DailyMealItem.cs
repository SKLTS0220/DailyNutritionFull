public class DailyMealItem
{
    public int Id { get; set; }

    // 外鍵
    public int DailyMealId { get; set; }
    public DailyMeal DailyMeal { get; set; }

    public int FoodItemId { get; set; }
    public FoodItem FoodItem { get; set; }
}
