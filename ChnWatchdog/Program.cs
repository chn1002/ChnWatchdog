using ChnWatchdog.src;
using System;
using System.IO;
using System.Windows.Forms;

namespace ChnWatchdog
{
    static class Program
    {
        public static xmlDataConfig gXMLDataConfig;
        public static LogProcess mLogProc;

        static xmlDataProcess gXMLProcess;
        static MainSystemForm mMainsystemForm;
        static NetworkProcess mNetworkProecss;

        static SystemManager mSystemManager;
        static WatchdogManager mWatchdogManager;

        private const string configFileName = "/watchDog.xml";

        [STAThread]
        static void Main()
        {
            gXMLProcess = new xmlDataProcess();
            xmlConfigStart(false);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mMainsystemForm = new MainSystemForm();
            mLogProc = new LogProcess(gXMLDataConfig.SystemConfig.SaveLogFile, mMainsystemForm);

            mSystemManager = new SystemManager(mMainsystemForm);
            mWatchdogManager = new WatchdogManager(mMainsystemForm);
            mNetworkProecss = new NetworkProcess(mMainsystemForm);

            mLogProc.LogWrite(LogProcess.LOGTYPE.NORMAL, "Start watchdog Program");
            Application.Run(mMainsystemForm);
        }

        public static void initSystem()
        {
            if (gXMLDataConfig.WatchdogConfig.Enable)
            {
                mLogProc.DebugLog("WatchDog Enable");
                mWatchdogManager.startWatchdogManager();
            }
            else
            {
                mLogProc.DebugLog("WatchDog disable");
            }

            if (gXMLDataConfig.SystemConfig.Enable)
            {
                mLogProc.DebugLog("SystemManager Enable");
                mSystemManager.startSystemManager();
            }
            else
            {
                mLogProc.DebugLog("SystemManager disable");
            }

            if(gXMLDataConfig.NetworkConfig.Enable)
            {
                mNetworkProecss.startServer();
                mLogProc.DebugLog("NetworkConfig Enable");
            }
            else
            {
                mNetworkProecss.stopServer();
                mLogProc.DebugLog("NetworkConfig Disable");
            }

            if(gXMLDataConfig.NetworkWatchDogConfig.Enable)
            {
                mNetworkProecss.startNetworkWatchdogManager();
                mLogProc.DebugLog("NetworkWatchdog Enable");
            }
            else
            {
                mLogProc.DebugLog("NetworkWatchdog Disble");
            }

            if (gXMLDataConfig.LogServerConfig.Enable)
            {
                mLogProc.startServer(gXMLDataConfig.LogServerConfig.Port);
                mLogProc.DebugLog("LogServer Enable");
            }
            else
            {
                mLogProc.DebugLog("LogServer Disble");
            }
        }

        private static void xmlConfigStart(bool save)
        {
            gXMLDataConfig = new xmlDataConfig { ProgrameName = "WatchDog" };

            string fileName = AppDomain.CurrentDomain.BaseDirectory + configFileName;
            FileInfo configFile = new System.IO.FileInfo(fileName);

            if (save)
            {
                gXMLProcess.XMLCreate(gXMLDataConfig, fileName);

                return;
            }

            if (configFile.Exists)
            {
                gXMLDataConfig = gXMLProcess.XMLDeserialize(fileName, gXMLDataConfig);
            }
            else
            {
                gXMLProcess.XMLCreate(gXMLDataConfig, fileName);
            }
        }

        internal static void systemShutdown()
        {
            mWatchdogManager.stopWatchdogManager();
            mSystemManager.stopSystemManager();
            mNetworkProecss.stopNetworkWatchdogManager();
            mLogProc.stopServer();

            mLogProc.LogWrite(LogProcess.LOGTYPE.NORMAL, "Shutdown watchdog Program");

            Application.Exit();
            Application.ExitThread();
            Environment.Exit(0);
        }

        internal static void checkWatchdog()
        {
            if(mWatchdogManager.getWatchdogEnable())
            {
                mWatchdogManager.stopWatchdogManager();
            }
            else
            {
                mWatchdogManager.startWatchdogManager();
            }
        }

        public static xmlDataConfig GetXmlDataConfig()
        {
            return gXMLDataConfig;
        }
    }
}
