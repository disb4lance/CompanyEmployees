using Contracts;
using LoggerService;
using Repository;
using Service.Contracts;
using Service;
using static System.Net.WebRequestMethods;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
//        Мы используем базовые настройки политики CORS.
//        Но мы должны быть более
//ограничивает эти настройки в производственной среде.
//Вместо метода AllowAnyOrigin(), который разрешает запросы от любого
//источник, мы можем использовать WithOrigins("https://example.com"), который будет
//разрешить запросы только из этого конкретного источника.Также вместо
//AllowAnyMethod(), который разрешает все методы HTTP, мы можем использовать
//WithMethods("POST", "GET"), который разрешает только определенные методы HTTP.
//Кроме того, вы можете внести те же изменения в AllowAnyHeader().
//используя, например, метод WithHeaders("accept", "contenttype"), чтобы разрешить только определенные заголовки.
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
             {
             });
        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureSqlContext(this IServiceCollection services,
        IConfiguration configuration) =>
            services.AddDbContext<RepositoryContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

        public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
            builder.AddMvcOptions(config => config.OutputFormatters.Add(new
            CsvOutputFormatter()));
    }
}
