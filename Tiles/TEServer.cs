using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.DataStructures;

namespace WirelessTeleporter.Tiles
{
    class TEServer : ModTileEntity
    {
        internal string name;
        internal int capacity;
        internal int serverID;
        internal int style;
        internal Point16 position = new Point16(-1, -1);
        internal IList<Point16> teleports = new List<Point16>();

        private void InitAfterPlace(int i, int j, int stil, int id)
        {
            TEServer tmp = (TEServer) TileEntity.ByID[id];
            tmp.style = stil;
            tmp.serverID = id;
            tmp.capacity = (stil + 1) * 2;
            tmp.name = "Server" + id;
            tmp.position = new Point16(i, j);
            UpdateWorld(tmp.position, tmp.capacity);

         }

        public static Point16 GetTopLeft(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int posx = i - ((tile.frameX % 54) / 18);
            int posy = j - (tile.frameY / 18);
            return new Point16(posx, posy);
        }

        private void UpdateWorld(Point16 pos,int cap)
        {
            WirelessWorld.servers.Add(pos);
            WirelessWorld.activeServers++;
            WirelessWorld.totalCapacity += cap;

        }

        public string GetServerInfo()
        {
            string info="";
            info =  "Name : " + name + "\n";
            info += "Cap  : " + capacity;
            return info;
        }

        public override void Update()
        {
        }

        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
        }

        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"name", name},
                {"capacity", capacity},
                {"serverID", serverID},
                {"style", style},
                {"teleports", teleports},
                {"pos", position }
            };
        }

        public override void Load(TagCompound tag)
        {
            name = tag.Get<string>("name");
            capacity = tag.Get<int>("capacity");
            serverID = tag.Get<int>("serverID");
            style = tag.Get<int>("style");
            teleports = tag.GetList<Point16>("teleports");
            position = tag.Get<Point16>("pos");
            UpdateWorld(position, capacity);
        }

        public override bool ValidTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == mod.TileType<WirelessServer>() && tile.frameX % 54 == 0 && tile.frameY / 18 == 0;
        }

        public override void OnKill()
        {
            if (WirelessWorld.activeServers > 0) { WirelessWorld.activeServers--; }
            foreach (Point16 pos in teleports)
            {
                TETeleport tel = (TETeleport)TileEntity.ByPosition[pos];
                tel.connectedTo = new Point16(-1, -1);
            }
            base.OnKill();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
                NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }
            int id = Place(i, j);
            
            InitAfterPlace(i, j, style, id);
            return id;
        }
    }
}
