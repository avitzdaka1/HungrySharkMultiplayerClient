using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private Game game;
       
        

        public NetworkConnection(Game game, string appID, string name, string serverIP, int port)
        {
            this.game = game;
            this.appID = appID;
            this.serverIP = serverIP;
            this.port = port;

            loginInformation = new NetworkLoginInformation()
            {
                Name = name
            };

            netClient = new NetClient(new NetPeerConfiguration(appID));
            netClient.Start();
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
            
            outMsg = netClient.CreateMessage();
            outMsg.Write((byte)PacketType.Login);
            outMsg.WriteAllProperties(loginInformation);
            netClient.Connect(serverIP,port, outMsg);



              return EstablishInfo();
         //   return true;
        }

        public void getNewEnemy(Enemy[] enemies, NetIncomingMessage msg)
        {
            
           
                string tempName = msg.ReadString();
                int tempId = msg.ReadInt32();
                Enemy temp = new Enemy(game);
                temp.setName(tempName);
                temp.setId(tempId);

            enemies[tempId] = temp;
                
            
        }
        

        public bool SendCoords(double X, double Y)
        {
            outMsg = netClient.CreateMessage();
            outMsg.Write((byte)PacketType.Data);
            outMsg.Write(Player.id);
            outMsg.Write(X);
            outMsg.Write(Y);
            netClient.SendMessage(outMsg, NetDeliveryMethod.UnreliableSequenced,12);

            return true;

        }

        private bool EstablishInfo()
        {
            var time = DateTime.Now;
            NetIncomingMessage inc;
            while(true)
            {
                if(DateTime.Now.Subtract(time).Seconds > 5)
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
                                var tid = inc.ReadInt32();
                                Player.id = tid;
                                Player.name = Player.name + tid.ToString();
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



        public void Update(Enemy[] enemies)
        {
            NetIncomingMessage msg;
            if((msg = netClient.ReadMessage()) == null)
                return;
            byte header = msg.ReadByte();
            if (header == (byte)PacketType.NewPlayer)
                getNewEnemy(enemies,msg);
            else
            if(header == (byte)PacketType.Data && msg.SequenceChannel == 9)
            {
                int id = msg.ReadInt32();
                double x = msg.ReadDouble();
                double y = msg.ReadDouble();
                Vector2 pos = new Vector2((float)x, (float)y);
                enemies[id].Position = pos; 
            }
        }




    }
}
