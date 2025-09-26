// File: SCMS.WebApp/Program.cs
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SCMS.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SystemAdmin", policy => policy.RequireRole("SystemAdmin"));
    options.AddPolicy("CanteenManager", policy => policy.RequireRole("CanteenManager"));
    options.AddPolicy("CanteenStaff", policy => policy.RequireRole("CanteenStaff"));
});

builder.Services.AddScoped<AuthHeaderHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddSingleton<TokenService>();

// Cấu hình các HttpClient cho các service khác
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
});

builder.Services.AddHttpClient<MenuService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
}).AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient<OrderService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
}).AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient<ReportService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
}).AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient<UserService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
}).AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient<WalletService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
}).AddHttpMessageHandler<AuthHeaderHandler>();

// <<< THAY ĐỔI: Đăng ký NotificationService và HttpClient của nó theo cách chuẩn
// 1. Đăng ký NotificationService là Singleton để nó có thể chia sẻ trạng thái
builder.Services.AddSingleton<NotificationService>();
// 2. Cấu hình một HttpClient có tên là "AuthorizedClient" mà service sẽ sử dụng
builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? "https://localhost:7063");
}).AddHttpMessageHandler<AuthHeaderHandler>();
// <<< KẾT THÚC THAY ĐỔI

// Đăng ký các service không cần HttpClient
builder.Services.AddSingleton<CartService>();
builder.Services.AddSingleton<ToastService>();
builder.Services.AddScoped<ConfirmDialogService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();