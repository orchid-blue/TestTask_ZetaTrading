using Microsoft.EntityFrameworkCore;
using ZetaTrading.API.Domain.Persistence.Contexts;
using ZetaTrading.API.Domain.Persistence.Repositories;
using ZetaTrading.API.Domain.Services;
using ZetaTrading.API.Domain.Repositories;
using ZetaTrading.API.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<INodeService, NodeService>();
builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddScoped<IJournalRecordService, JournalRecordService>();
builder.Services.AddScoped<IJournalRecordRepository, JournalRecordRepository>();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Test v1");
});
app.UseExceptionHandler("/error");
app.MapControllerRoute(
    name: "default",
    pattern: "swagger");
//app.MapGet("/", () => "Hello World!");

app.Run();
