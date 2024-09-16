using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace CompanyEmployees.ContextFactory
{

    //Этот код нужен для создания контекста базы данных во время выполнения команд миграций или других операций на этапе разработки.
    //Entity Framework CLI будет использовать этот класс для создания контекста и выполнения команд на базе данных.
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args) // метод фабрики, который создает экземпляр контекста базы данных RepositoryContext
        {
            var configuration = new ConfigurationBuilder() // создается объект конфигурации, который загружает настройки из файла
            .SetBasePath(Directory.GetCurrentDirectory()) // SetBasePath указывает на текущую директорию проекта
            .AddJsonFile("appsettings.json")
            .Build();
            var builder = new DbContextOptionsBuilder<RepositoryContext>() // Создается объект DbContextOptionsBuilder, который настраивает параметры для контекста базы данных
                .UseSqlServer(configuration.GetConnectionString("sqlConnection"), // Этот метод настраивает использование базы данных SQL Server, получая строку подключения из файла конфигурации appsettings.json
                                                                                  // через ключ sqlConnection. Этот ключ должен присутствовать в секции "ConnectionStrings" в файле appsettings.json.
                    b => b.MigrationsAssembly("CompanyEmployees")); // миграции будут храниться в проекте
            return new RepositoryContext(builder.Options);
        }
    }

}

