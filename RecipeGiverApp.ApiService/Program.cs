using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ReciveGiverApp.Database.Data;
using ReciveGiverApp.BL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.
builder.Services.AddProblemDetails();

// Register ConnectionManager as a singleton
builder.Services.AddSingleton<ConnectionManager>();

//Add connection between ICategoryService and CategoryService
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IRecipeService, RecipeService>();
builder.Services.AddTransient<IIngredientService, IngredientService>();
builder.Services.AddTransient<IRecipeIngredientService, RecipeIngredientService>();
builder.Services.AddTransient<IFavouritesService, FavouritesService>();
builder.Services.AddTransient<IDailyMealsService, DailyMealsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.MapDefaultEndpoints();

app.Run();
