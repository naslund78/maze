using System;
using System.Collections.Generic;
using System.Text;

namespace Maze.Core.Services
{
    public class LoggingService
    {
        private LogLevel logLevel;
        public LoggingService(LogLevel _logLevel)
        {
            logLevel = _logLevel;
        }
        public enum LogLevel
        {
            None, Info
        }
        public void LogInfo(string message)
        {
            if (logLevel == LogLevel.Info)
                System.Console.WriteLine(message);
        }
        public void LogError(string message)
        {
            System.Console.WriteLine("Error"  + message);
        }
    }
}
