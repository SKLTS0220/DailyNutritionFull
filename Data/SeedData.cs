using System.Linq;

public static class SeedData
{
    public static void Init(AppDbContext db)
    {
        // 如果已經有菜色，就不要再 seed
        if (db.FoodItems.Any())
            return;

        db.FoodItems.AddRange(
            new FoodItem { Name = "白飯", FoodType = FoodType.Main },
            new FoodItem { Name = "麵包", FoodType = FoodType.Main },
            new FoodItem { Name = "酸種麵包", FoodType = FoodType.Main },
            new FoodItem { Name = "地瓜飯", FoodType = FoodType.Main },
            new FoodItem { Name = "魚肉", FoodType = FoodType.Main },
            new FoodItem { Name = "雞胸肉", FoodType = FoodType.Main },
            new FoodItem { Name = "豬肉", FoodType = FoodType.Main },
            new FoodItem { Name = "牛肉", FoodType = FoodType.Main },
            new FoodItem { Name = "豆腐", FoodType = FoodType.Main },
            new FoodItem { Name = "高麗菜", FoodType = FoodType.Side },
            new FoodItem { Name = "地瓜葉", FoodType = FoodType.Side },
            new FoodItem { Name = "空心菜", FoodType = FoodType.Side },
            new FoodItem { Name = "水蓮", FoodType = FoodType.Side },
            new FoodItem { Name = "南瓜", FoodType = FoodType.Side },
            new FoodItem { Name = "小黃瓜", FoodType = FoodType.Side },
            new FoodItem { Name = "絲瓜", FoodType = FoodType.Side },
            new FoodItem { Name = "苦瓜", FoodType = FoodType.Side },
            new FoodItem { Name = "山藥", FoodType = FoodType.Side },
            new FoodItem { Name = "秋葵", FoodType = FoodType.Side },
            new FoodItem { Name = "綠色花椰菜", FoodType = FoodType.Side },
            new FoodItem { Name = "水煮蛋", FoodType = FoodType.Side },
            new FoodItem { Name = "蒸蛋", FoodType = FoodType.Side },
            new FoodItem { Name = "荷包蛋", FoodType = FoodType.Side }
        );

        db.SaveChanges();
    }
}
