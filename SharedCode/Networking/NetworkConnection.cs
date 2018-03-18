using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossPlatform.Fruits;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private Logger logger;
        private DataLog dataLogger;
        private HashSet<Apple> apples;
       
        

        public bool Active { get; set; }

        public NetworkConnection(Game game, string appID, string name, string serverIP, int port, Enemy[] enemies, HashSet<Apple> apples)
        {
            this.game = game;
            this.appID = appID;
            this.serverIP = serverIP;
            this.port = port;
            this.enemies = enemies;
            logger = new Logger();
            dataLogger = new DataLog();
            this.apples = apples;
            

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


        public void Start()
        {

            outMsg = netClient.CreateMessage();
            outMsg.Write((byte)PacketType.Login);
            outMsg.WriteAllProperties(loginInformation);
            netClient.Connect(serverIP, port, outMsg);

            System.Threading.Thread.Sleep(300);

            Update();
           
        }


        public void SendCoords(double X, double Y)
        {
            outMsg = netClient.CreateMessage();
            outMsg.Write((byte)PacketType.Input);
            outMsg.Write(Player.id);
            outMsg.Write(X);
            outMsg.Write(Y);
            netClient.SendMessage(outMsg, NetDeliveryMethod.Unreliable,0);


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
                        int tid = msg.ReadInt32();
                        string tmpName = msg.ReadString();
                        double x = msg.ReadDouble();
                        double y = msg.ReadDouble();
                        Vector2 tempPos = new Vector2((float)x, (float)y);
                        if (enemies[tid] == null)
                        {
                            Enemy temp = new Enemy(game);
                            temp.setId(tid);
                            temp.setName(tmpName);
                            temp.Position = tempPos;
                            enemies[tid] = temp;
                        }
                            else
                        {
                            if (enemies[tid].Position.X != tempPos.X)
                            {
                                if (enemies[tid].Position.X > tempPos.X)
                                    enemies[tid].setDirection(1);
                                else
                                    enemies[tid].setDirection(0);
                            }
                            enemies[tid].Position = new Vector2(MathHelper.Lerp(enemies[tid].Position.X, tempPos.X, 0.8f), MathHelper.Lerp(enemies[tid].Position.Y, tempPos.Y, 0.8f));
                        }
                        break;
                    case PacketType.AllPlayers:
                        getAllPlayers(msg);

                        break;
                    case PacketType.Login:
                        
                                var temID = msg.ReadInt32();
                                Player.id = temID;
                                Player.name = Player.name + temID.ToString();
                                getAllPlayers(msg);
                        break;
                    case PacketType.Fruit:
                        var amount = msg.ReadInt32();
                        for (int i = 0; i < amount; i++)
                        {
                            int xx = msg.ReadInt32();
                            int yy = msg.ReadInt32();
                            apples.Add(new Apple(xx, yy));
                        }

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
                    logger.Log("Enemy id: " + enemies[tid].getId() + " Name: " + enemies[tid].getName() + " X: " + enemies[tid].Position.X + " Y: " + enemies[tid].Position.Y);
                    
                }
            }

        }

        public void Eat(Player player, List<SoundEffect> snd, Random rnd)
        {
            Apple apl = null;
            foreach(Apple ap in apples)
            {
                if(Vector2.Distance(ap.getPosition(),player.Position) < 50)
                {
                    int num = rnd.Next(0, 4);
                    snd[num].Play();
                    var outmsg = netClient.CreateMessage();
                    outmsg.Write((byte)PacketType.Eat);
                    outmsg.Write(ap.getX());
                    outmsg.Write(ap.getY());
                    apl = ap;
                    netClient.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
               
                   
                }
                
            }
            if(apl != null)
                apples.Remove(apl);
            logger.Log(enemies.Length.ToString());


        }
       
    }
        
   
        




   
}
