using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // ✅ 這裡負責註冊所有服務（DI）
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();

        // ✅【關鍵】註冊 AppDbContext
		var dbPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "daily_nutrition.db");
        services.AddDbContext<AppDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));
    }

    // ✅ 這裡負責 HTTP Pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
		
		// ✅ 初始化資料庫與 SeedData
		using (var scope = app.ApplicationServices.CreateScope())
		{
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			db.Database.EnsureCreated();
			SeedData.Init(db);
		}

    }
}
