using System;

namespace Labs.api
{
    class Log
    {
        public static void console(String text, params object[] args)
        {
            Console.WriteLine(text, args);
        }
    }
}
