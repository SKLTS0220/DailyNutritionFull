using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;

public class HistoryModel : PageModel
{
    private readonly AppDbContext _db;

    public DailyMeal? Today { get; set; }
    public DailyMeal? Yesterday { get; set; }

    public HistoryModel(AppDbContext db)
    {
        _db = db;
    }

    public void OnGet()
    {
        Today = _db.DailyMeals
            .FirstOrDefault(d => d.Date == DateTime.Today);

        Yesterday = _db.DailyMeals
            .FirstOrDefault(d => d.Date == DateTime.Today.AddDays(-1));
    }
}
