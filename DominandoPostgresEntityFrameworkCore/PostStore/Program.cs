using Microsoft.EntityFrameworkCore;
using Poststore.Models;
using PostStore.Data;
using PostStore.Requests;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new Exception("Connection string not found.");

builder.Services.AddDbContext<PostStoreDbContext>(x=> x.UseNpgsql(connectionString));

var app = builder.Build();

var j =1;

app.MapPost("/v1/categories", async (PostStoreDbContext context, CreateCategoryRequest request) =>
{
    j++;

   var category = new Category
   {
     Heading = new ()
     {
      Title = $"{request.Title} {j}",
      Slug = $"{request.Slug}-{j}"  
     } 
   };

   await context.Categories.AddAsync(category);
   
   context.SaveChanges();
    
    return Results.Created($"/v1/categories/{category.Id}", category);
});

app.MapGet("/v1/categories", async (PostStoreDbContext context) =>
{
    var categories = await context
        .Categories
        .AsNoTracking()
        .ToListAsync();
    
    return Results.Ok(categories);
});

var i =0;

app.MapPost("/v1/products", async (PostStoreDbContext context, CreateProductRequest request) =>
{   
    i++;

   var product = new Product
   {
       Heading = new ()
       {
         Title = $"{request.Title} {i}",
         Slug = $"{request.Slug}-{i}"
       },      
       CategoryId = j,        
       CreatedAtUtc = DateTime.UtcNow,
       UpdatedAtUtc = DateTime.UtcNow,
       IsActive = true,
       DefaultLanguage = request.DefaultLanguage,
       Translations = request.Translations       
   };
   
   await context.Products.AddAsync(product);

   context.SaveChanges();

   return Results.Created($"/v1/products/{product.Id}", product);
});

app.MapGet("/v1/products", async (PostStoreDbContext context) =>
{
    var products = await context
        .Products
        .AsNoTracking()
        .ToListAsync();
    
    return Results.Ok(products);
});

app.Run();
