var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(); // đăng kí dịch vụ MVC

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{   
    app.UseHttpsRedirection();//bỏ bắt buộc https trong môi trường dev
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection(); // đieu huong trang
app.UseStaticFiles(); // dung cac file tinh như hmlcss

app.UseRouting(); //mapping mvc 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

//log
app.Use(async (context,next) => {
    Console.WriteLine($"Request: {context.Request.Path}");
    await next();
});

app.UseAuthorization(); //xac thực users

app.MapRazorPages();// UI

app.Run();
