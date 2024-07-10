using AuthIdentity.Infrastructure;
using General.Installer;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// builder.Host.UseSerilog();
builder.Services.AddInstallers(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services  = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<IdentityDbContext>();
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();