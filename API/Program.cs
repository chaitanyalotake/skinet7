using API.Extensions;
using API.Middleware;
using Core.Entities.Identity;
using Infrastrcture.Data;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServives(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
var app = builder.Build();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseSwaggerDocumentation();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var identityContext = services.GetRequiredService<AppIdentityDbContext>();
var userManager = services.GetRequiredService<UserManager<AppUser>>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await identityContext.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
    await AppIdentityDbContextSeed.SeedUserAsync(userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "Error occured during migration");
}
app.Run();
