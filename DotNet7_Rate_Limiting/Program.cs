using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



#region Fixed window
//Sabit vaxt araliginda requestleri limitleyen algoritmdir
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Fixed", config =>
    {
        config.Window = TimeSpan.FromSeconds(10);
        config.PermitLimit = 4;
        config.QueueLimit = 2;
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});
#endregion

#region Sliding
//Fixed window ile oxhsardir amma sabit vaxtin yarisinda diger periodun requestlerini
//qarşılar.
builder.Services.AddRateLimiter(options =>
{
    options.AddSlidingWindowLimiter("Sliding", config =>
    {
        config.Window = TimeSpan.FromSeconds(10);
        config.PermitLimit = 2;
        config.QueueLimit = 2;
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        config.SegmentsPerWindow = 2;
    });
});
#endregion

#region Token Bucket
//Her periodda qebul edilecek request sayısı qeder token yaratmaqdadır. Bu tokenlar
//periodda istifade olunduysa digər perioddan borc alına bilər. Lakin her periodda
//token yaratma dəyəri nədirsə, o qədər token yaradılacaq və bu zaman rate limit işleyecek.
//Hər periodun maximum token limiti  verilen sabit sayı qədər olacaqdır!;

builder.Services.AddRateLimiter(options =>
{
    options.AddTokenBucketLimiter("Token", config =>
    {
        config.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        config.TokenLimit = 2;
        config.QueueLimit = 2;
        config.TokensPerPeriod = 2;
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

#endregion

#region Concurrency
builder.Services.AddRateLimiter(options =>
{
    options.AddConcurrencyLimiter("Concurrency", config =>
    {
        config.PermitLimit = 2;
        config.QueueLimit = 2;
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});
#endregion

var app = builder.Build();

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
