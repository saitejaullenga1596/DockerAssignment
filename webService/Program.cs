using webservice.Interface;
using webservice.Model;
using webservice.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<PostService>();
// configure cache service to read baseUri
builder.Services.Configure<CacheServiceConfiguration>(x => builder.Configuration.GetSection("CacheService").Bind(x));

//configure data service
builder.Services.Configure<DataServiceConfiguration>(x => builder.Configuration.GetSection("DataService").Bind(x));

//adding services to DI Container
builder.Services.AddSingleton<IPostService, PostService>();

var app = builder.Build();

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
