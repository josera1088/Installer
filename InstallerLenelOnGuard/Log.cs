using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace InstallerLenelOnGuard
{
    public class Log
    {
        static ConcurrentQueue<string> LogQueue = new ConcurrentQueue<string>();
        private string fullpath = "";
        #region Singleton
        static Log instance;
        static Log() {            
        }
        public static Log GetInstance()
        {
            if (instance == null)
            {
                instance = new Log();
                instance.fullpath = AppDomain.CurrentDomain.BaseDirectory;
            }
            return instance;
        }
        #endregion Singleton        

        #region Methods
        public void ChangeFullPath(string newPath)
        {
            this.fullpath = newPath;
        }

        public void DoLog(string log, string module, Exception ex = null)
        {
            try
            {
                if (ex != null) LogQueue.Enqueue($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - {module}: {log} Ex: {ex}");
                else LogQueue.Enqueue($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - {module}: {log}");
                WriteInFile("AMSInstallationLog", "log");
            }
            catch (Exception) { }
        }

        public void WriteInFile(string fileName, string fileExtension)
        {
            try
            {
                if (!LogQueue.IsEmpty)
                {
                    List<string> lines = new List<string>();
                    while (LogQueue.TryDequeue(out string line))
                    {
                        lines.Add(line);
                    }
                    string filePath = instance.fullpath + $"\\{fileName}.{fileExtension}";
                    int c = 15;//MAX_LOG_RETRY
                    while (c > 0)
                    {
                        try
                        {
                            File.AppendAllLines(filePath, lines);
                            c = 0;
                        }
                        catch (Exception) {
                        }
                        c--;
                        if (c > 0)
                            Thread.Sleep(new Random().Next(70, 150));
                    }
                }
            }
            catch (Exception) { }
        }

        #endregion Methods
    }
}
