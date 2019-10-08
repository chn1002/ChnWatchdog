using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChnWatchdog.src
{
    [XmlRoot("chnProjectConfig")]
    public class xmlDataConfig
    {
        [XmlAttribute]
        public string ProgrameName;

        [XmlAttribute]
        public bool DebugMode;

        [XmlAttribute]
        public int MaxNetworkwatchdog = 1;

        public SysConf SystemConfig;
        public WatchDogConf WatchdogConfig;
        public NetWatchDogCof NetworkWatchDogConfig;
        public NetConf NetworkConfig;
        public LogServerConf LogServerConfig;

        public xmlDataConfig()
        {
            DebugMode = true;
            SystemConfig = new SysConf();
            WatchdogConfig = new WatchDogConf();
            NetworkWatchDogConfig = new NetWatchDogCof();
            NetworkConfig = new NetConf();
            LogServerConfig = new LogServerConf();
        }

        public class SysConf
        {
            public SysConf()
            {
                Enable = true;
                Timer = 30;
                CpuOverCheckRate = 90;
                MemoryOverCheckRate = 1000;
                SaveLogFile = true;
            }

            [XmlElement("Enable")]
            public bool Enable { get; set; }

            [XmlElement("Timer")]
            public int Timer { get; set; }

            [XmlElement("CpuOverCheckRate")]
            public int CpuOverCheckRate { get; set; }

            [XmlElement("MemoryOverCheckRate")]
            public int MemoryOverCheckRate { get; set; }

            [XmlElement("SaveLogFile")]
            public bool SaveLogFile { get; set; }

        }

        public class NetWatchDogCof
        {
            public NetWatchDogCof()
            {
                Enable = true;
                Port = 5555;
                WatchDogTimeGap = 60;
                RunProgrameName = false;
                AfterRunProgrameTime = 10;
                RunProgramPath = "c:\\";
                WatchdogRebootEnable = false;
                RebootAction = 1;

            }

            [XmlElement("Enable")]
            public bool Enable { get; set; }

            [XmlElement("Port")]
            public int Port { get; set; }

            [XmlElement("RunProgrameName")]
            public bool RunProgrameName { get; set; }

            [XmlElement("AfterRunProgrameTime")]
            public int AfterRunProgrameTime { get; set; }

            [XmlElement("RunProgramPath")]
            public string RunProgramPath { get; set; }

            [XmlElement("WatchdogRebootEnable")]
            public bool WatchdogRebootEnable { get; set; }

            [XmlElement("RebootAction")]
            public int RebootAction { get; set; }

            [XmlElement("WatchDogTimeGap")]
            public int WatchDogTimeGap { get; set; }
        }

        public class WatchDogConf
        {
            public WatchDogConf()
            {
                Enable = true;
                ProcessName = "Test";
                WatchDogTimeGap = 60;
                RunProgrameName = false;
                AfterRunProgrameTime = 10;
                RunProgramePath = "c:\\";
                WatchdogRebootEnable = false;
                RebootAction = 1;
                //0: Shutdown
                //1: Reboot
            }

            [XmlElement("Enable")]
            public bool Enable { get; set; }

            [XmlElement("ProcessName")]
            public string ProcessName { get; set; }

            [XmlElement("RunProgrameName")]
            public bool RunProgrameName { get; set; }

            [XmlElement("AfterRunProgrameTime")]
            public int AfterRunProgrameTime { get; set; }

            [XmlElement("RunProgramePath")]
            public string RunProgramePath { get; set; }

            [XmlElement("WatchdogRebootEnable")]
            public bool WatchdogRebootEnable { get; set; }

            [XmlElement("RebootAction")]
            public int RebootAction { get; set; }

            [XmlElement("WatchDogTimeGap")]
            public int WatchDogTimeGap { get; set; }

        }

        public class NetConf
        {
            public NetConf()
            {
                Enable = false;
                NetworkAddress = "127.0.0.1";
                Port = 12000;
            }

            [XmlElement("Enable")]
            public bool Enable { get; set; }

            [XmlElement("NetworkAddress")]
            public string NetworkAddress { get; set; }

            [XmlElement("Port")]
            public int Port { get; set; }
        }

        public class LogServerConf
        {
            public LogServerConf()
            {
                Enable = false;
                NetworkAddress = "127.0.0.1";
                Port = 12100;
            }

            [XmlElement("Enable")]
            public bool Enable { get; set; }

            [XmlElement("NetworkAddress")]
            public string NetworkAddress { get; set; }

            [XmlElement("Port")]
            public int Port { get; set; }
        }
    }
}
