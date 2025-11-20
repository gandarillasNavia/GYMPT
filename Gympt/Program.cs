using Gympt.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Configura el HttpClient para que sepa la dirección base de la API y lo inyecta en DisciplineApiClient
var disciplineApiUrl = builder.Configuration["ApiSettings:DisciplineApiUrl"];
builder.Services.AddHttpClient<DisciplineApiClient>(client =>
{
    client.BaseAddress = new Uri(disciplineApiUrl);
});
var membershipApiUrl = builder.Configuration["ApiSettings:MembershipApiUrl"];
builder.Services.AddHttpClient<MembershipApiClient>(client =>
{
    client.BaseAddress = new Uri(membershipApiUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();