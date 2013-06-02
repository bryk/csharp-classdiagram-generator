using System;
using System.Configuration;
using System.IO;

namespace Server.Utils
{
    public sealed class Logger : IDisposable
    {
        private StreamWriter sw;

        private Logger()
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + ConfigurationManager.AppSettings["logFile"];
            sw = new StreamWriter(filePath, true);
        }


        private static readonly Lazy<Logger> lazy = new Lazy<Logger>(() => new Logger());

        public static Logger Instance { get { return lazy.Value; } }


        public void Log(string text)
        {
#if DEBUG
            Console.WriteLine(string.Format("[{0}] {1}", DateTime.Now, text));
#endif
            sw.WriteLine(string.Format("[{0}] {1}", DateTime.Now, text));
            sw.Flush();
        }

        public void LogDebug(string text)
        {
#if DEBUG
            sw.WriteLine(string.Format("[{0}] [debug] {1}", DateTime.Now, text));
            sw.Flush();
#endif
        }

        public void LogError(string text)
        {
            sw.WriteLine(string.Format("[{0}] [error] {1}", DateTime.Now, text));
            sw.Flush();
        }

        public void LogFatal(string text)
        {
            sw.WriteLine(string.Format("[{0}] [fatal] {1}", DateTime.Now, text));
            sw.Flush();
        }

        #region IDisposable Members

        public void Dispose()
        {
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        #endregion
    }
}
