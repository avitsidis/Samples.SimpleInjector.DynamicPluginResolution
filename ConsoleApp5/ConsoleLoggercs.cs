using Common;
using System;

namespace ConsoleApp5
{
    class ConsoleLogger : ILogger
    {
        private void Log(string s)
        {
            Console.WriteLine(s);
        }

        public void Debug(string s)
        {
            Log(s);
        }

        public void Error(string s)
        {
            Log(s);
        }

        public void Info(string s)
        {
            Log(s);
        }

        public void Warning(string s)
        {
            Log(s);
        }
    }
}
