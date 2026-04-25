using Domain.Settings;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace Application.Helper
{
    public class LogWriter: ILogWriter
    {
        private readonly PeriodicLoginSettings _periodicSetting;

        public LogWriter(IOptionsSnapshot<PeriodicLoginSettings>  periodicSetting)
        {
            _periodicSetting = periodicSetting.Value;
        }

        public void LogExceptionToFile(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            string errorlineNo = ex.StackTrace != null ? ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7) : string.Empty;
            string errorMsg = ex.Message;
            string errorInnerMsg = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
            string extype = ex.GetType().ToString();

            try
            {
                //string folderName = _periodicSetting.LogFilePath;
                //string filepath = Path.Combine(folderName, "LPPC-Logins\\");
                string filepath = _periodicSetting.LogFilePath;

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath = filepath + "Exception_" + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name

                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line +
                        "Error Line No :" + " " + errorlineNo + line + 
                        "Error Message:" + " " + errorMsg + line +
                        "Exception Type:" + " " + extype + line + 
                        "Error InnerException :" + " " + errorInnerMsg + line;

                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }

        public void WriteLogToFile(string message)
        {
            var line = Environment.NewLine + Environment.NewLine;

            try
            {
                //string folderName = _periodicSetting.LogFilePath;
                //string filepath = Path.Combine(folderName, "LPPC-Logins\\");
                string filepath = _periodicSetting.LogFilePath;

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                filepath = filepath + "LogInfo_" + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name

                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string information = "Log Written Date:" + " " + DateTime.Now.ToString() + line +
                        "Information: " + " " + message + line;

                    sw.WriteLine("----------- Application Login Details on " + " " + DateTime.Now.ToString() + "---------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(information);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }

            }
            catch (Exception e)
            {
                e.ToString();

            }
        }
    }

    public interface ILogWriter
    {
        void LogExceptionToFile(Exception ex);
        void WriteLogToFile(string message);
    }
}
