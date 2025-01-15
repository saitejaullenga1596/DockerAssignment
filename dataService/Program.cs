using dataService;
using dataService.Interface;
using dataService.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure DBContext class 
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register PostService as Scoped to ensure it works with DbContext (which is Scoped)
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope()) { var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>(); dbContext.Database.Migrate(); }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
