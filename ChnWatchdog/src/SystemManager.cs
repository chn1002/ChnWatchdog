using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ChnWatchdog.src
{
    class SystemManager
    {
        Thread SystemManagerTh;
        MainSystemForm mMainsystemForm;
        SerialProcess mSerialPort;
        float cpuInt = 0;
        float memoryInt = 0;

        public SystemManager(MainSystemForm mainSystemForm)
        {
            ThreadStart systemManagerThStart = new ThreadStart(systemManagerThread);
            SystemManagerTh = new Thread(systemManagerThStart);

            mMainsystemForm = mainSystemForm;
            mSerialPort = new SerialProcess();
        }

        public float CurrentCpuUsage { get => cpuInt; }
        public float CurrentMemoryUsage { get => memoryInt;}

        public void startSystemManager()
        {
            SystemManagerTh.Start();
        }

        public void stopSystemManager()
        {
            if (SystemManagerTh.ThreadState == System.Threading.ThreadState.Running)
            {
                SystemManagerTh.Abort();
            }
        }

        private void systemManagerThread()
        {
            int i = 0;
            PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter AvailableRam = new PerformanceCounter("Memory", "Available MBytes");

            string[] serialPort;
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                cpuInt = cpu.NextValue();
                memoryInt = AvailableRam.NextValue();
                serialPort = mSerialPort.getUserSerialPort();

                mMainsystemForm.writeCpuUsage(cpuInt + " %");
                mMainsystemForm.writeMemoryUsage(memoryInt + " MB");

                for (i = 0; i < serialPort.Length; i++)
                {
                    sb.Append(serialPort[i]);
                }

                mMainsystemForm.writeUseSerialPort(sb.ToString());

                if (cpuInt > Program.gXMLDataConfig.SystemConfig.CpuOverCheckRate)
                {
                    Program.mLogProc.LogWrite(LogProcess.LOGTYPE.SYSTEM, "CPU Usage " + cpuInt + "%");
                }

                if (memoryInt < Program.gXMLDataConfig.SystemConfig.MemoryOverCheckRate)
                {
                    Program.mLogProc.LogWrite(LogProcess.LOGTYPE.SYSTEM, "Memory Available " + memoryInt + "%");
                }

                sb.Clear();
                Thread.Sleep(Program.gXMLDataConfig.SystemConfig.Timer * 100);
            }
        }
    }
}
