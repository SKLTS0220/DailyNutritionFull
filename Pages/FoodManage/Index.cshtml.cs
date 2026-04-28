using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

public class FoodManageModel : PageModel
{
    private readonly AppDbContext _db;

    public List<FoodItem> Foods { get; set; } = new();

    [BindProperty]
    public string NewFoodName { get; set; } = "";

    public FoodManageModel(AppDbContext db)
    {
        _db = db;
    }

	[BindProperty(SupportsGet = true)]
	public FoodType? FilterType { get; set; }

	public void OnGet()
	{
		var query = _db.FoodItems.AsQueryable();

		if (FilterType.HasValue)
		{
			query = query.Where(f => f.FoodType == FilterType.Value);
		}

		Foods = query
			.OrderBy(f => f.FoodType)
			.ThenBy(f => f.Name)
			.ToList();
	}
	
    public IActionResult OnPost()
    {
        if (!string.IsNullOrWhiteSpace(NewFoodName))
        {
            _db.FoodItems.Add(new FoodItem { Name = NewFoodName });
            _db.SaveChanges();
        }
        return RedirectToPage();
    }

    public IActionResult OnPostToggle(int id)
    {
        var food = _db.FoodItems.First(f => f.Id == id);
        food.IsEnabled = !food.IsEnabled;
        _db.SaveChanges();
        return RedirectToPage();
    }
	
	public IActionResult OnPostToggleType(int id)
	{
		var food = _db.FoodItems.First(f => f.Id == id);

		// 主菜 ↔ 配菜 切換
		food.FoodType = food.FoodType == FoodType.Main
			? FoodType.Side
			: FoodType.Main;

		_db.SaveChanges();
		return RedirectToPage();
	}

}
