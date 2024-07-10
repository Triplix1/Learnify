using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication()
    .AddJwtBearer("IdentityApiKey", options =>
    {
        options.Authority = builder.Configuration["IdentityUrl"];
        options.RequireHttpsMetadata = false;
    });

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy
                .WithOrigins(builder.Configuration.GetSection("AllowedHosts").Get<string[]>() ?? new string[] { })
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
    });

builder.Services.AddOcelot(builder.Configuration);
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseSwaggerForOcelotUI(option => 
{ 
    option.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot();
await app.RunAsync();