using System;

public enum FoodType
{
    Main = 1,   // 主菜
    Side = 2    // 配菜
}

public class FoodItem
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    // 是否顯示在長輩頁面
    public bool IsEnabled { get; set; } = true;

    // 主菜 / 配菜
    public FoodType FoodType { get; set; } = FoodType.Side;
}