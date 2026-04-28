using Microsoft.EntityFrameworkCore;
using System;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<FoodItem> FoodItems => Set<FoodItem>();

    public DbSet<DailyMeal> DailyMeals => Set<DailyMeal>();

	public DbSet<DailyMealItem> DailyMealItems { get; set; }
}
