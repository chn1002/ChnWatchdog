using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ChnWatchdog.src
{
    class NetworkProcess
    {
        private MainSystemForm mMainsystemForm;
        private ClientCommunication netClient;
        private UdpCommunction netServer;
        private bool networkEnable = false;
        bool networkWatchdogFunctionEnable;
        Thread NetworkWatchDogTh;


        public NetworkProcess(MainSystemForm mainSystemForm)
        {
            mMainsystemForm = mainSystemForm;

            // For UDP : Datagrame
            netClient = new ClientCommunication(false);
            ClientCommunication.OnNetworkReceive += OnNetworkReceive;

            netServer = new UdpCommunction();
            UdpCommunction.OnNetworkReceive += OnNetworkReceive;

            networkWatchdogFunctionEnable = false;
            NetworkWatchDogTh = new Thread(NetworkWatchDogThread);
        }

        private void OnNetworkReceive(string data)
        {
            Program.mLogProc.DebugLog("Data RECV: " + data);

            netServer.Send(data + " echo");
        }

        internal void startServer()
        {
            netServer.Server("10.56.29.20", 6000);
            networkEnable = true;
            mMainsystemForm.NetworkWatchDogStatusGUI(networkEnable);
        }

        internal void stopServer()
        {
            networkEnable = false;
            mMainsystemForm.NetworkWatchDogStatusGUI(networkEnable);
        }

        public void startNetworkWatchdogManager()
        {
            //            udpClient.StartClient("127.0.0.1", Program.gXMLDataConfig.NetworkWatchDogConfig.Port);
//            netClient.StartClient("10.56.29.20", 6000);
  //          NetworkWatchDogTh.Start();
            networkWatchdogFunctionEnable = true;
        }

        public void stopNetworkWatchdogManager()
        {
            if (NetworkWatchDogTh.ThreadState == System.Threading.ThreadState.Running)
            {
                NetworkWatchDogTh.Abort();
            }

            networkWatchdogFunctionEnable = false;
        }

        void checkWatchdogTh()
        {
            if (networkWatchdogFunctionEnable)
            {
                networkWatchdogFunctionEnable = false;
            }
            else
            {
                networkWatchdogFunctionEnable = true;
            }
        }

        private void NetworkWatchDogThread()
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();

            Thread.Sleep(1000);

            while (true)
            {
                Program.mLogProc.DebugLog("Check Data" + i);

                if (networkWatchdogFunctionEnable == false)
                {
                }
                else
                {
                    netClient.Send("Message " + i++);
                    netClient.Receive();

                    sb.Clear();
                }

                Thread.Sleep(Program.gXMLDataConfig.NetworkWatchDogConfig.WatchDogTimeGap * 100);
            }
        }

        public bool getNetworkWatchdogEnable()
        {
            return networkWatchdogFunctionEnable;
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
