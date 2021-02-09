using System;
using System.Collections.Generic;
using System.IO;

namespace Coinbase.BalanceMonitor.Infrastructure
{
    public static class Logger
    {
        public static void LogError(string message, Exception exception)
        {
            var lines = new List<string>
                        {
                            $"[{DateTime.UtcNow:s}] {message}" 
                        };

            var exceptionLines = exception.ToString().Split(Environment.NewLine);

            foreach (var line in exceptionLines)
            {
                lines.Add($"    {line}");
            }

            File.AppendAllLines("Logs.txt", lines);
        }
    }
}