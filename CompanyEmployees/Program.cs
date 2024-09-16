using CompanyEmployees;
using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;


var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// Add services to the container.

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); ;

var app = builder.Build();
//var logger = app.Services.GetRequiredService<ILoggerManager>();
//app.ConfigureExceptionHandler(logger);
app.UseExceptionHandler(opt => { });

//if (app.Environment.IsProduction())
//    app.UseHsts();

//if (app.Environment.IsDevelopment())
//    app.UseDeveloperExceptionPage();
//else
//    app.UseHsts(); // добавит промежуточное программное обеспечение для использования HSTS, которое добавляет Заголовок Strict-Transport-Security
app.UseHttpsRedirection(); // перенаправление с http на https
app.UseStaticFiles(); // по умолчанию wwwrot
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});  // app.UseForwardedHeaders() пересылает заголовки прокси-сервера в текущий запрос.
app.UseCors("CorsPolicy");
app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Hello from the middleware component.");
//    await next.Invoke();
//    Console.WriteLine($"Logic after executing the next delegate in the Use method");
//});


//app.Map("/usingmapbranch", builder =>
//{
//    builder.Use(async (context, next) =>
//    {
//        Console.WriteLine("Map branch logic in the Use method before the next delegate");
       
//        await next.Invoke();
//        Console.WriteLine("Map branch logic in the Use method after the next delegate");
//    });
//    builder.Run(async context =>
//    {
//        Console.WriteLine($"Map branch response to the client in the Run method");
//        await context.Response.WriteAsync("Hello from the map branch.");
//    });
//});


//app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder =>
//    {
//        builder.Run(async context =>
//        {
//            await context.Response.WriteAsync("Hello from the MapWhen branch.");
//        });
//    });

//app.Run(async context =>
//{
//    Console.WriteLine($"Writing the response to the client in the Run method");
//    context.Response.StatusCode = 200;
//    await context.Response.WriteAsync("Hello from the middleware component.");
//});

app.MapControllers();
app.Run();
