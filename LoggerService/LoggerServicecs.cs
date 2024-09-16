using Contracts;
using NLog;

namespace LoggerService
{
    // Отладка и диагностика
    //Отслеживание ошибок и инцидентов
    //Анализ производительности
    //Безопасность
    //Аудит и мониторинг
    //Поддержка и обслуживание
    //Планирование и улучшение
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger(); // Этот метод возвращает логгер, настроенный для текущего класса LoggerManager
        public LoggerManager()
        {
        }
        public void LogDebug(string message) => logger.Debug(message);
        public void LogError(string message) => logger.Error(message);
        public void LogInfo(string message) => logger.Info(message);
        public void LogWarn(string message) => logger.Warn(message);
    }

}
