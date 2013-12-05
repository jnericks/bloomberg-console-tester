using System;
using System.Collections.Generic;
using BenchmarkPlus.CommonDomain.Extensions;

namespace BloombergConsoleTester
{
    public static class Log
    {
        public static Action<string> Logger;
        static IList<string> unlogged = new List<string>();

        public static void Error(string message)
        {
            DoLog(@"=====ERROR=====");
            DoLog(Tab() + message);
        }

        public static void Exception(Exception exception)
        {
            DoLog(@"=====EXCEPTION=====");
            DoLog(Tab() + exception);
            DoLog(Tab() + exception.Message);

            if (exception.InnerException == null) return;

            DoLog(@"=====INNER EXCEPTION=====");
            DoLog(Tab() + exception);
            DoLog(Tab() + exception.Message);
        }

        public static void Index(int indexKey, string indexName, string indexTicker)
        {
            DoLog(@"{0}: Key = {1}; Ticker = {2}".FormatWith(indexName, indexKey, indexTicker));
        }

        public static void Info(string message)
        {
            DoLog(message);
        }

        public static void Timeout(decimal seconds)
        {
            Error(@"Failed :-(  TIMEOUT after {0}s)".FormatWith(seconds));
        }

        static string Tab()
        {
            return @"    ";
        }

        static void DoLog(string msg)
        {
            if (Logger == null)
            {
                unlogged.Add(@"*" + msg);
                return;
            }

            foreach (var m in unlogged)
                Logger(m);

            Logger(msg);

            unlogged.Clear();
        }
    }
}