using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;


namespace AndroidVersion
{
    class NetworkConnection
    {
        private NetClient netClient;
        private string appID;
        private NetworkLoginInformation loginInformation;
        private string serverIP;
        private int port;
        private NetOutgoingMessage outMsg;
        

        public NetworkConnection(string appID, string name, string serverIP, int port)
        {
            this.appID = appID;
            this.serverIP = serverIP;
            this.port = port;

            loginInformation = new NetworkLoginInformation()
            {
                Name = name
            };

            netClient = new NetClient(new NetPeerConfiguration(appID));
            outMsg = netClient.CreateMessage();
        }

        
        public bool Stop()
        {
            outMsg = netClient.CreateMessage();
            outMsg.Write("Cya@@@");
            netClient.Disconnect("Cya!!!");
            return true;
        }
        

        public bool Start()
        {
            netClient.Start();
            outMsg.Write((byte)PacketType.Login);
            outMsg.WriteAllProperties(loginInformation);
            netClient.Connect(serverIP,port, outMsg);

            

            return EstablishInfo();
              
        }

        public bool SendCoords(double X, double Y)
        {
            outMsg = netClient.CreateMessage();
            outMsg.Write((byte)PacketType.Data);
            outMsg.WriteAllProperties(loginInformation);
            outMsg.Write(X);
            outMsg.Write(Y);
            netClient.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);

            return true;

        }

        private bool EstablishInfo()
        {
            var time = DateTime.Now;
            NetIncomingMessage inc;
            while(true)
            {
                if(DateTime.Now.Subtract(time).Seconds > 1)
                {
                    return false;
                }
                if((inc = netClient.ReadMessage()) == null)
                    continue;
                switch(inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        var data = inc.ReadByte();
                        if(data == (byte) PacketType.Login)
                        {
                            var accepted = inc.ReadBoolean();
                            if (accepted)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        break;
                }
                
            }
        }
    }
}
