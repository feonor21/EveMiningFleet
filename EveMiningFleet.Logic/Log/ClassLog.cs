using System;
using System.IO;

namespace EveMiningFleet.Logic.Log
{
    public static class ClassLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ex"></param>
        /// <param name="_folderlog"></param>
        public static void writeException(Exception _ex,string _folderlog = "log/")
        {
            string error = "";
            while (_ex != null)
            {
                error += _ex.GetType().FullName + "\n";
                error += "Message : " + _ex.Message + "\n";
                error += "StackTrace : " + _ex.StackTrace + "\n";
                _ex = _ex.InnerException;
            }

            writeLog(error, _folderlog);
        }


        static readonly object AppendAllTextLock = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public static void writeLog(string _text, string _folderlog = "log/")
        {
            // lock (AppendAllTextLock)
            // {
            //     Directory.CreateDirectory(_folderlog);
            //     string pathlog = _folderlog + DateTime.Now.ToString("yyyyMMdd") + ".log";
            //     Console.WriteLine(DateTime.Now.ToString("HH mm ss ff") + "\t" + _text);
            //     File.AppendAllText(pathlog, DateTime.Now.ToString("HH mm ss ff") + "\t" + _text + "\n");
            // }

            Console.WriteLine(DateTime.Now.ToString("HH mm ss ff") + "\t" + _text);
            
            
        }

        // public static void purgelog(string _folderlog = "log/")
        // {
        //     Directory.CreateDirectory(_folderlog);
        //     DirectoryInfo dir = new DirectoryInfo(_folderlog);
        //     DateTime testDate = DateTime.Now.AddDays(-7);
        //     foreach (FileInfo f in dir.GetFiles())
        //     {
        //         DateTime fileAge = f.LastWriteTime;
        //         if (fileAge < testDate)
        //         {
        //             File.Delete(f.FullName);
        //             ClassLog.writeLog("File " + f.Name + " is older than today, deleted...", _folderlog);
        //         }
        //     }
        // }



    }
}