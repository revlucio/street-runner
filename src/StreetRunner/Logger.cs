using System;
using System.Diagnostics;

namespace StreetRunner
{
    public static class Logger 
    {
        public static Stopwatch _timer;

        public static void LogTime(string message)
        {
            if (_timer == null) {
                _timer = Stopwatch.StartNew();
            }
            //Console.WriteLine(_timer.ElapsedMilliseconds.ToString("00000") + " " + message);
        }
    }
}
