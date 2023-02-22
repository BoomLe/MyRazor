
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using EFWebRazor.models;

var builder = WebApplication.CreateBuilder(args);


// Add DBcontext Connecting to SQL MyBlogContext

builder.Services.AddDbContext<MyDbContext>(options=>
{
    var stringConnecting = builder.Configuration.GetConnectionString("MyDbContext");
    options.UseSqlServer(stringConnecting);
    
});
// Add services to the container.
builder.Services.AddRazorPages();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
