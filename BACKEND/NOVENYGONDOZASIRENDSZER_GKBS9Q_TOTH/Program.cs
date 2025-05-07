using NOVENYGONDOZASIRENDSZER_GKBS9Q_TOTH.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NovenyDbContext>();
builder.Services.AddTransient<INovenyRepository, NovenyRepository>();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

var app = builder.Build();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);

app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x =>
    x.AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithOrigins("http://127.0.0.1:5500"));


app.Run();
