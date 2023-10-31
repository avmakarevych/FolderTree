using Microsoft.EntityFrameworkCore;
using FolderTree.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DirectoryHierarchyContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Directory}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DirectoryHierarchyContext>();
    DbInitializer.Initialize(context);
}

app.Run();