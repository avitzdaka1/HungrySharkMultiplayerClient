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
        private Enemy[] enemies;

        public bool Active { get; set; }

        public NetworkConnection(Game game, string appID, string name, string serverIP, int port, Enemy[] enemies)
        {
            this.game = game;
            this.appID = appID;
            this.serverIP = serverIP;
            this.port = port;
            this.enemies = enemies;

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
            netClient.Connect(serverIP, port, outMsg);



            return EstablishInfo();
            //   return true;
        }

        public void getNewEnemy(NetIncomingMessage msg)
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
            outMsg.Write((byte)PacketType.Input);
            outMsg.Write(Player.id);
            outMsg.Write(X);
            outMsg.Write(Y);
            netClient.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered,1);

            return true;

        }

        private bool EstablishInfo()
        {
            var time = DateTime.Now;
            NetIncomingMessage inc;
            while (true)
            {
                if (DateTime.Now.Subtract(time).Seconds > 5)
                {
                    return false;
                }
                if ((inc = netClient.ReadMessage()) == null)
                    continue;
                switch (inc.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        var data = inc.ReadByte();
                        if (data == (byte)PacketType.Login)
                        {
                            Active = inc.ReadBoolean();
                            if (Active)
                            {
                                var tid = inc.ReadInt32();
                                Player.id = tid;
                                Player.name = Player.name + tid.ToString();
                                getAllPlayers(inc);
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



        public void Update()
        {
            NetIncomingMessage msg;
            while ((msg = netClient.ReadMessage()) != null)
            {
                if (msg.MessageType != NetIncomingMessageType.Data)
                    continue;
                var packType = (PacketType)msg.ReadByte();
                switch (packType)
                {
                    case PacketType.NewPlayer:
                        Enemy temp = new Enemy(game);
                        string tmpName = msg.ReadString();
                        int tmpID = msg.ReadInt32();
                        temp.setName(tmpName);
                        temp.setId(tmpID);
                        Vector2 s = new Vector2(40, 40);
                        temp.Position = s;
                        enemies[tmpID] = temp;
                        break;
                    case PacketType.AllPlayers:
                        getAllPlayers(msg);

                        break;
                }


            }

           

        }
        public void getAllPlayers(NetIncomingMessage inc)
        {
            var count = inc.ReadInt32();
            for(int i = 0; i < count ; i ++)
            {
                var tid = inc.ReadUInt32();
                var tName = inc.ReadString();
                var tX = inc.ReadDouble();
                var tY = inc.ReadDouble();
                Vector2 tempPos = new Vector2((float)tX, (float)tY);

                if (tid == Player.id)
                    continue;
                if (enemies[tid] != null)
                {
                    enemies[tid].Position = tempPos;
                }
                else
                {
                    Enemy temp = new Enemy(game);
                    temp.setName(tName);
                    temp.setId((int)tid);
                    temp.Position = tempPos;
                    enemies[tid] = temp;
                }
            }
        }
    }
        

            /*
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
                if(enemies[id] == null)
                {
                    enemies[id] = new Enemy(game);
                    enemies[id].Position = pos;
                }
                enemies[id].Position = pos; 
            }
             */
        




   
}
