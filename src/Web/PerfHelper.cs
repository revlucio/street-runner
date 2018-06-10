using System;

namespace StreetRunner.Web
{
    public static class PerfHelper
    {
        public static void PrintTime(string message)
        {
            var time = DateTime.Now.ToString("O");
            Console.WriteLine(time + " " + message);
        }
    }
}