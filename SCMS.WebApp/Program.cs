// File: SCMS.WebApp/Program.cs
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SCMS.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// --- BẮT ĐẦU KHU VỰC ĐĂNG KÝ SERVICE ---

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorization(options =>
{
    // Policy này yêu cầu người dùng phải có vai trò "SystemAdmin"
    options.AddPolicy("SystemAdmin", policy =>
        policy.RequireRole("SystemAdmin"));

    // Policy này yêu cầu người dùng phải có vai trò "CanteenManager"
    options.AddPolicy("CanteenManager", policy =>
        policy.RequireRole("CanteenManager"));

    // Policy này yêu cầu người dùng phải có vai trò "CanteenStaff"
    options.AddPolicy("CanteenStaff", policy =>
        policy.RequireRole("CanteenStaff"));
});

builder.Services.AddSingleton<TokenService>();
// --- BẮT ĐẦU SỬA ĐỔI TOÀN DIỆN ---

// 1. Đăng ký AuthHeaderHandler để nó có thể được thêm vào các HttpClient
builder.Services.AddScoped<AuthHeaderHandler>();

// 2. Đăng ký AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// 3. Đăng ký TỪNG service cần gọi API và cấu hình HttpClient cho nó.
//    Phương pháp này đảm bảo mỗi service sẽ nhận được một HttpClient
//    đã được gắn sẵn AuthHeaderHandler.

builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
});
// Lưu ý: AuthService không cần AuthHeaderHandler vì request login không cần token.

builder.Services.AddHttpClient<MenuService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
})
.AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient<OrderService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
})
.AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient<ReportService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
})
.AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient<UserService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
})
.AddHttpMessageHandler<AuthHeaderHandler>();


// 4. Đăng ký các service không cần HttpClient
builder.Services.AddSingleton<CartService>();
builder.Services.AddSingleton<ToastService>();

// --- KẾT THÚC SỬA ĐỔI TOÀN DIỆN ---

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();