
using Microsoft.EntityFrameworkCore;
using OneFlowDomain.Tables;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<OneFlowDBContext>(
            opt =>
            {
                opt.UseSqlServer(configuration.GetValue<string>("EntityFrameworkCore:ConnectionString"), builder =>
                {
                    builder.EnableRetryOnFailure(
                      maxRetryCount: 5,
                      maxRetryDelay: TimeSpan.FromSeconds(30),
                      errorNumbersToAdd: null);

                });
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
    );
//builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
//{
//    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
//}));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
//app.UseCors("corsapp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
