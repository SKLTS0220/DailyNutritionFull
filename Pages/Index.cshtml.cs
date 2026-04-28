using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

	[BindProperty]
	public DateTime SelectedDate { get; set; } = DateTime.Today;

    // ✅ 對應 Index.cshtml 的 Model.Foods
    public List<FoodItem> Foods { get; set; } = new List<FoodItem>();
	
	[BindProperty(SupportsGet = true)]
	public MealType Meal { get; set; } = MealType.Lunch;

	[BindProperty]
	public int? MainFoodId { get; set; }

	[BindProperty]
	public List<int> SideFoodIds { get; set; } = new();

    // ✅ 接收 checkbox 勾選結果
    [BindProperty]
    public List<int> Ids { get; set; } = new List<int>();

    // ✅ 對應 Index.cshtml 的 Model.Result
    public NutritionStatus? Result { get; set; }
	
	public List<FoodItem> MainFoods { get; set; } = new();
	public List<FoodItem> SideFoods { get; set; } = new();
	
	public int TodayMealCount { get; set; }
	public int TotalMealCount => 4;
	
	public HashSet<MealType> TodayFilledMeals { get; set; } = new();
	
	public string TodayText =>
		DateTime.Today.ToString("yyyy/MM/dd（dddd）",
			new CultureInfo("zh-TW"));


    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

	public void OnGet()
	{
		MainFoods = _db.FoodItems
			.Where(f => f.IsEnabled && f.FoodType == FoodType.Main)
			.OrderBy(f => f.Name)
			.ToList();

		SideFoods = _db.FoodItems
			.Where(f => f.IsEnabled && f.FoodType == FoodType.Side)
			.OrderBy(f => f.Name)
			.ToList();
	
		TodayFilledMeals = _db.DailyMeals
			.Where(m => m.Date == DateTime.Today)
			.Select(m => m.MealType)
			.ToHashSet();
		
		TodayMealCount = TodayFilledMeals.Count;
	}
	
	public IActionResult OnPost()
	{
		// ✅ 1.餐別檢查
		if (!Enum.TryParse<MealType>(Request.Form["Meal"], out var selectedMeal))
		{
			ModelState.AddModelError(string.Empty, "請選擇餐別");
			LoadFoods();          // 重新載入主菜 / 配菜清單
			return Page();
		}
		
		
		// ✅ 2. 後端防呆：一定要選主菜
		if (!MainFoodId.HasValue)
		{
			ModelState.AddModelError(string.Empty, "請選擇一樣主菜");
			LoadFoods();
			return Page();
		}

		
		// ✅ 3. 建立餐紀錄
		Foods = _db.FoodItems.ToList();
		Result = NutritionStatus.Safe;

		var meal = new DailyMeal
		{
			Date = SelectedDate.Date,		// ✅ 存使用者選的日期
			MealType = selectedMeal,
			Status = NutritionStatus.Safe
		};
		
		// 防呆：確認主菜真的存在
		if (MainFoodId.HasValue &&
			!_db.FoodItems.Any(f => f.Id == MainFoodId.Value))
		{
			throw new Exception($"主菜 FoodItemId={MainFoodId.Value} 不存在");
		}

		// 防呆：確認所有配菜都存在
		foreach (var id in SideFoodIds)
		{
			if (!_db.FoodItems.Any(f => f.Id == id))
				throw new Exception($"配菜 FoodItemId={id} 不存在");
		}		
				
		_db.DailyMeals.Add(meal);
		_db.SaveChanges();
		

		// ✅ 4. 存主菜
		if (MainFoodId.HasValue)
		{
			_db.DailyMealItems.Add(new DailyMealItem
			{
				DailyMealId = meal.Id,
				FoodItemId = MainFoodId.Value
			});
		}

		//  ✅ 5. 存配菜
		foreach (var sideId in SideFoodIds)
		{
			_db.DailyMealItems.Add(new DailyMealItem
			{
				DailyMealId = meal.Id,
				FoodItemId = sideId
			});
		}
		
		// 資料寫入完成
		_db.SaveChanges();		
		
		// ✅ 設定成功提示
		TempData["SuccessMessage"] = $"{selectedMeal} 已完成";


		// ✅ 6. 成功後一定要回傳
		// PRG：回到新的 GET 頁面
		return RedirectToPage();

	}
	
	public string FriendlyDayText
	{
		get
		{
			var today = DateTime.Today;
			var culture = new System.Globalization.CultureInfo("zh-TW");
			return $"今天 {today:yyyy/MM/dd}（{today.ToString("dddd", culture)}）";
		}
	}
	
	private void LoadFoods()
	{
		MainFoods = _db.FoodItems
			.Where(f => f.IsEnabled && f.FoodType == FoodType.Main)
			.OrderBy(f => f.Name)
			.ToList();

		SideFoods = _db.FoodItems
			.Where(f => f.IsEnabled && f.FoodType == FoodType.Side)
			.OrderBy(f => f.Name)
			.ToList();
	}
	
}
