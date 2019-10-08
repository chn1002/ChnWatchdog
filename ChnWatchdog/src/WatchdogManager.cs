
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ChnWatchdog.src
{
    class WatchdogManager
    {
        private MainSystemForm mMainsystemForm;
        bool watchdogFunctionEnable;
        Thread watchDogTh;


        public WatchdogManager(MainSystemForm mainsystemForm)
        {
            mMainsystemForm = mainsystemForm;
            watchdogFunctionEnable = false;

            watchDogTh = new Thread(watchDogThread);
        }

        /**
         * Start Watchdog Manager(Thread)
         **/
        public void startWatchdogManager()
        {
            watchDogTh.Start();
            watchdogFunctionEnable = true;

            mMainsystemForm.watchDogStatusGUI(watchdogFunctionEnable);
        }

        /**
         * Stop Watchdog Manager(Thread)
         **/
        public void stopWatchdogManager()
        {
            if (watchDogTh.ThreadState == System.Threading.ThreadState.Running)
            {
                watchDogTh.Abort();
            }

            watchdogFunctionEnable = false;
            mMainsystemForm.watchDogStatusGUI(watchdogFunctionEnable);
        }

        void checkWatchdogTh()
        {
            if (watchdogFunctionEnable)
            {
                watchdogFunctionEnable = false;
            }
            else
            {
                watchdogFunctionEnable = true;
            }

            mMainsystemForm.watchDogStatusGUI(watchdogFunctionEnable);
        }

        /**
         *  Process Running Check Function
         *  processName 
         **/
        public bool checkProcessRunning(string processName)
        {
            Process[] pro = Process.GetProcesses();

            for (int i = 0; i < pro.Length; i++)
            {
                if (pro[i].MainWindowHandle != IntPtr.Zero)
                {
                    if (pro[i].MainWindowTitle == "")
                        continue;

                    if (pro[i].ProcessName == processName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /**
         * Watchdof Main Function (Thread)
         **/
        private void watchDogThread()
        {
            bool processStatus = false;
            Process[] pro;
            StringBuilder sb = new StringBuilder();

            Thread.Sleep(1000);

            while (true)
            {
                if (watchdogFunctionEnable == false)
                {

                }
                else
                {
                    processStatus = false;
                    pro = Process.GetProcesses();

                    for (int i = 0; i < pro.Length; i++)
                    {
                        if (pro[i].MainWindowHandle != IntPtr.Zero)
                        {
                            if (pro[i].MainWindowTitle == "")
                                continue;

                            if (pro[i].ProcessName == Program.gXMLDataConfig.WatchdogConfig.ProcessName)
                            {
                                processStatus = true;
                            }

                            sb.AppendLine(pro[i].ProcessName);
                        }
                    }

                    if (processStatus)
                    {
                        mMainsystemForm.writeWatchdogProcessStatus("Running");
                    }
                    else
                    {
                        mMainsystemForm.writeWatchdogProcessStatus(Program.gXMLDataConfig.WatchdogConfig.ProcessName + " About");
                        Program.mLogProc.LogWrite(LogProcess.LOGTYPE.WATCHDOG, Program.gXMLDataConfig.WatchdogConfig.ProcessName + " : About");
                        if (Program.gXMLDataConfig.WatchdogConfig.RunProgrameName)
                        {
                            Thread.Sleep(Program.gXMLDataConfig.WatchdogConfig.AfterRunProgrameTime * 100);

                            if (Program.gXMLDataConfig.WatchdogConfig.WatchdogRebootEnable)
                            {
                                showDown(Program.gXMLDataConfig.WatchdogConfig.RebootAction);
                            }
                            else
                            {
                                Process.Start(Program.gXMLDataConfig.WatchdogConfig.RunProgramePath);
                                Program.mLogProc.LogWrite(LogProcess.LOGTYPE.WATCHDOG, "run : " + Program.gXMLDataConfig.WatchdogConfig.RunProgramePath);
                            }
                        }
                    }

                    mMainsystemForm.writeWatchdogProcessName(Program.gXMLDataConfig.WatchdogConfig.ProcessName);
                    mMainsystemForm.writeUpdateDate(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    mMainsystemForm.writeUpdateDate(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    mMainsystemForm.writeProcessList(sb.ToString());
                    sb.Clear();
                }

                Thread.Sleep(Program.gXMLDataConfig.WatchdogConfig.WatchDogTimeGap * 100);
            }
        }

        public bool getWatchdogEnable()
        {
            return watchdogFunctionEnable;
        }

        private void showDown(int watchDogRebootAction)
        {
            if (watchDogRebootAction == 1)
            {
                System.Diagnostics.Process.Start("ShutDown", "-s"); //shutdown
            }
            else
            {
                System.Diagnostics.Process.Start("ShutDown", "-r"); //restart
            }
        }
    }
}
