using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ChnWatchdog.src
{
    class LogProcess
    {
        bool mSaveLogfile = false;
        MainSystemForm mMainsystemForm;
        private UdpCommunction netServer;
        bool logServerEnable;

        public enum LOGTYPE
        {
            NORMAL,
            SYSTEM,
            WATCHDOG,
            NETLOG
        }

        public LogProcess(Boolean saveLog, MainSystemForm mainSystemForm)
        {
            mSaveLogfile = saveLog;
            mMainsystemForm = mainSystemForm;
            netServer = new UdpCommunction();
            UdpCommunction.OnNetworkReceive += OnNetworkReceive;
        }

        private void OnNetworkReceive(string data)
        {
            Program.mLogProc.DebugLog("LogProcess Data RECV: " + data);
            Program.mLogProc.LogWrite(LOGTYPE.NETLOG, data);
        }

        internal void startServer(int port)
        {
            netServer.Server("10.56.29.20", port);
            logServerEnable = true;
        }

        internal void stopServer()
        {
            netServer.CloseServer();
            logServerEnable = false;
        }

        public void DebugLog(string log)
        {
            System.Diagnostics.Debug.WriteLine(log);

            if (mMainsystemForm != null)
            {
                mMainsystemForm.writeDebug(log);
            }
        }
        
        public void LogWrite(LOGTYPE type, string str)
        {
            string FilePath = Environment.CurrentDirectory + @"\Log\Log_" + type.ToString() + "_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string DirPath = Environment.CurrentDirectory + @"\Log";
            string temp;

            if (mSaveLogfile  != true)
            {
                temp = string.Format("[{0}] {1}", DateTime.Now, str);

                Console.WriteLine(temp);
                return;
            }
            else
            {
                temp = string.Format("[{0}] {1}", DateTime.Now, str);
            }

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (!di.Exists) Directory.CreateDirectory(DirPath);
                if (!fi.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("[{0}] {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
