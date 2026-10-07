using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using TestMinimalApi.Application;
using TestMinimalApi.Application.Interfaces.Repositories;
using TestMinimalApi.Application.Interfaces.Services;
using TestMinimalApi.Application.Services;
using TestMinimalApi.Application.Validators;
using TestMinimalApi.Infrastructure;
using TestMinimalApi.Infrastructure.Data;
using TestMinimalApi.Presentation.Api.EndPoints;
using TestMinimalApi.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);
// connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// ? IMPORTANT: Test environment ??? SqlServer ? register
if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<AppDbContext>(o =>
        o.UseSqlServer(connectionString));
}
builder.Services.AddValidatorsFromAssemblyContaining<RegionValidator>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
});
// 🔑 Authorization
builder.Services.AddAuthorization();
//var columnOptions = new ColumnOptions();

//// remove Properties column
//columnOptions.Store.Remove(StandardColumn.Properties);

// (optional) remove LogEvent also if you don't need it
// columnOptions.Store.Remove(StandardColumn.LogEvent);

//columnOptions.AdditionalColumns = new Collection<SqlColumn>
//{
//    new SqlColumn("SourceContext", SqlDbType.NVarChar),
//    new SqlColumn("RequestUrl", SqlDbType.NVarChar, dataLength: 255),
//    new SqlColumn("RequestType", SqlDbType.NVarChar, dataLength: 15),
//    new SqlColumn("UserId", SqlDbType.NVarChar, dataLength: 50)
//};

//Log.Logger = new LoggerConfiguration()
//    .WriteTo.MSSqlServer(
//        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
//        sinkOptions: new MSSqlServerSinkOptions
//        {
//            TableName = "SystemLogs",
//            AutoCreateSqlTable = true
//        },
//        restrictedToMinimumLevel: LogEventLevel.Warning,
//        columnOptions: columnOptions
//    )
//    .CreateLogger();
// Read Serilog config from appsettings.json
builder.Host.UseSerilog((ctx, lc) => lc .ReadFrom.Configuration(ctx.Configuration) );
var app = builder.Build();

// ===== Serilog automatic request logging =====
app.UseSerilogRequestLogging(options => {
    //options.MessageTemplate = "Handled {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms"; 
    options.EnrichDiagnosticContext = (diag, ctx) => 
    { 
        diag.Set("RequestUrl", ctx.Request.Path); 
        diag.Set("RequestType", ctx.Request.Method); 
        diag.Set("UserId", ctx.User?.Identity?.Name ?? "Anonymous");
    }; 
});
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test Minimal Api V1");
    c.RoutePrefix = string.Empty; // ဒါထည့်ရင် domain.onrender.com ဆိုတာနဲ့ Swagger တန်းပွင့်ပါမယ်
});
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwagger(c =>
//    {
//        c.RouteTemplate = "/swagger/{documentname}/swagger.json";
//    });

//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Test Minimal Api V1");
//    });
//    app.UseDeveloperExceptionPage();
//}
//else
//{
//    // Production ??? optional secure route
//    app.MapGet("/swagger", () => Results.Forbid());
//}
app.MapGoogleOAuthEndPoints();
app.MapTestEndPoints();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
public partial class Program { } // add at bottom of Program.cs
