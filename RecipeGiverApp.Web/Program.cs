using MudBlazor.Services;
using ReciveGiverApp.BL.Services;
using ReciveGiverApp.Database.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Add Razor components services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Only server components

// 2. Add MudBlazor services
builder.Services.AddMudServices();

// 3. Register application-specific services
builder.Services.AddSingleton<ConnectionManager>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IRecipeService, RecipeService>();
builder.Services.AddTransient<IIngredientService, IngredientService>();
builder.Services.AddTransient<IRecipeIngredientService, RecipeIngredientService>();
builder.Services.AddTransient<IFavouritesService, FavouritesService>();
builder.Services.AddTransient<IDailyMealsService, DailyMealsService>();

// 4. Configure HttpClient for API requests
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7381") });

// 5. Add controllers for API endpoints
builder.Services.AddControllers();

// 6. Add output caching if needed
builder.Services.AddOutputCache();

var app = builder.Build();

// Configure the HTTP request pipeline.

// 1. Error handling and security configurations
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// 2. HTTPS redirection and static files
app.UseHttpsRedirection();
app.UseStaticFiles();

// 3. Routing configuration
app.UseRouting();

// 4. Authorization (if needed)
// app.UseAuthentication();
// app.UseAuthorization();

app.UseAntiforgery();


app.MapControllers();

app.Run();
