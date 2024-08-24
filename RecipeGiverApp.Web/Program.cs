using MudBlazor.Services;
using RecipeGiverApp.Web.Components;
using ReciveGiverApp.BL.Services;
using ReciveGiverApp.Database.Data;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Register the database connection manager as a singleton
builder.Services.AddSingleton<ConnectionManager>();

// Register services as necessary
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IRecipeService, RecipeService>();
builder.Services.AddTransient<IIngredientService, IngredientService>();
builder.Services.AddTransient<IRecipeIngredientService, RecipeIngredientService>();
builder.Services.AddTransient<IFavouritesService, FavouritesService>();
builder.Services.AddTransient<IDailyMealsService, DailyMealsService>();

// Configure HttpClient for API requests
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7381") });


// Register controllers for API
builder.Services.AddControllers();

// Add Razor components and interactive server components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>();

app.Run();
