using SystemProvisioningPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add configuration from environment variables
builder.Configuration.AddEnvironmentVariables();

// Configure Azure DevOps settings from environment variables
builder.Services.Configure<AzureDevOpsSettings>(options =>
{
    options.ApiUrl = builder.Configuration["AZURE_DEVOPS_API_URL"] ?? 
        builder.Configuration["AzureDevOps:ApiUrl"] ?? 
        throw new InvalidOperationException("Azure DevOps API URL not configured");
    options.PersonalAccessToken = builder.Configuration["AZURE_DEVOPS_PAT"] ?? 
        builder.Configuration["AzureDevOps:PersonalAccessToken"] ?? 
        throw new InvalidOperationException("Azure DevOps PAT not configured");
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IAzureDevOpsService, AzureDevOpsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
